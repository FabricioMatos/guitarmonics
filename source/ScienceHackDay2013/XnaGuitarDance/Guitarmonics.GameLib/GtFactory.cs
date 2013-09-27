using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.WebServiceClient;

namespace Guitarmonics.GameLib
{
    #region Exceptions

    public class InvalidType : Exception { }

    public class TargetTypeNotFound : Exception { }

    #endregion

    #region SystemClock

    public interface IClock
    {
        DateTime CurrentDateTime { get; }
    }

    public class SystemClock : IClock
    {
        public DateTime CurrentDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }
    }

    #endregion


    /// <summary>
    /// Cria instâncias de classes a partir de interfaces.
    /// </summary>
    public class GtFactory
    {
        [ThreadStatic]
        private static GtFactory fInstance = null;

        public static GtFactory Instance
        {
            get
            {
                if (fInstance == null)
                    fInstance = new GtFactoryOffLine();
                    //fInstance = new GtFactoryOnLine();

                return fInstance;
            }
        }

        public GtFactory()
        {
            this.Clock = new SystemClock();
        }

        public IClock Clock { get; protected set; }
        private Dictionary<Type, Type> fMappings = new Dictionary<Type, Type>();

        /// <summary>
        /// Cria a intancia de um objeto a partir de uma interface.
        /// Se não for registrada uma classe explicitamente, a Fábrica procura uma classe no mesmo namespace 
        /// com o mesmo nome que a interface, sem o "I" inicial.
        /// </summary>
        /// <typeparam name="I">Interface</typeparam>
        /// <param name="pParametrosConstrutor">Parâmetros que serão usados na construção do objeto</param>
        /// <returns>Instancia do objeto</returns>
        public I Instantiate<I>(params object[] pConstructorParams)
            where I : class
        {
            var interfaceType = typeof(I);

            if (!interfaceType.IsInterface)
                throw new InvalidType();

            Type classType = this.GetClass<I>();

            return (I)Activator.CreateInstance(classType, pConstructorParams);
        }

        /// <summary>
        /// Implementa a lógica de busca da classe concreta que implementa a interface informada.
        /// Caso exista um mapeamento na memória, o mesmo será retornado, caso contrário, é retornado o tipo
        /// da classe de mesmo nome da interface, ex.:
        /// 
        /// ICidade -> Retorna Cidade.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <returns></returns>
        private Type GetClass<I>()
        {
            Type interfaceType = typeof(I);
            Type classType = fMappings.FirstOrDefault(c => c.Key == interfaceType).Value;

            if (classType == null)
            {
                classType = interfaceType.Assembly.GetType(this.GetClassName<I>());

                if (classType == null)
                    throw new TargetTypeNotFound();
            }

            return classType;
        }

        /// <summary>
        /// Adiciona um mapeamento personalizado à fábrica, fazendo com que o tipo que seja
        /// instanciado seja o desejado e não o padrão.
        /// </summary>
        /// <typeparam name="I">Interface</typeparam>
        /// <typeparam name="C">Classe</typeparam>
        public void AddMapping<I, C>()
            where C : class, I
            where I : class
        {
            var interfaceType = typeof(I);
            var classType = typeof(C);

            if (!interfaceType.IsInterface || !classType.IsClass)
                throw new InvalidType();

            this.CleanMappings<I>();

            fMappings.Add(interfaceType, classType);
        }

        /// <summary>
        /// Remove todos os mapeamentos personalizados que foram adicionados 
        /// à fábrica para a interface.
        /// </summary>
        /// <typeparam name="I">Interface</typeparam>
        public void CleanMappings<I>()
            where I : class
        {
            var interfaceType = typeof(I);

            if (!interfaceType.IsInterface)
                throw new InvalidType();

            fMappings.Remove(interfaceType);
        }

        /// <summary>
        /// Remove todos os mapeamentos personalizados que foram adicionados à fábrica.
        /// </summary>
        public void ClearAllMappings()
        {
            fMappings.Clear();
        }

        /// <summary>
        /// Retorna o nome da classe padrão para a interface.
        /// </summary>
        /// <param name="pNomeInterface">Nome completo da interface base</param>
        /// <returns>Nome da classe</returns>
        private string GetClassName<I>()
        {
            var fullName = typeof(I).FullName;
            var splitedName = fullName.Split('.');
            var originalName = splitedName[splitedName.Length - 1];
            var newName = originalName.Substring(1);

            return fullName.Replace(originalName, newName);
        }
    }

    public class GtFactoryOnLine : GtFactory
    {
        public GtFactoryOnLine()
        {
            this.AddMapping<IGameSongRepository, OnlineGameSongRepository>();
        }
    }


    public class GtFactoryOffLine : GtFactory
    {
        public GtFactoryOffLine()
        {
            this.AddMapping<IGameSongRepository, FileSystemGameSongRepository>();
        }
    }
}
