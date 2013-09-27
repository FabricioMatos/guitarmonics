using NUnit.Framework;
using Guitarmonics.GameLib;

namespace Guitarmonics.GameLib.Testes
{
    [TestFixture]
    public class GtFactoryTests
    {
        #region [ Setup ]

        /// <summary>
        /// Verifica se o objeto é do tipo informado.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="pObjeto">Instância do objeto</param>
        private void GarantirTipoDoObjeto<T>(object pObjeto)
        {
            Assert.IsNotNull(pObjeto);
            Assert.IsInstanceOfType(typeof(T), pObjeto);
        }

        #endregion

        /// <summary>
        /// Quando pedimos à fábrica para criar a instância de uma interface, 
        /// ela vai retornar a instância da classe concreta de mesmo nome que 
        /// esteja no mesmo Assembly da interface.
        /// 
        /// Ex.: IPessoa -> Retorna Pessoa.
        /// </summary>
        [Test]
        public void Instanciar()
        {
            var factory = new GtFactory();

            IPais pais = factory.Instantiate<IPais>();
            ITituloEleitor titulo = factory.Instantiate<ITituloEleitor>();

            this.GarantirTipoDoObjeto<Pais>(pais);
            this.GarantirTipoDoObjeto<TituloEleitor>(titulo);
        }

        /// <summary>
        /// Ao adicionar um mapeamento à Fábrica alteramos o comportamento padrão da mesma,
        /// sendo que toda vez que for solicitada uma instância do tipo ela retornará uma instância
        /// do tipo mapeado.
        /// </summary>
        [Test]
        public void AdicionarMapeamento()
        {
            var factory = new GtFactory();

            factory.AddMapping<IPais, PaisStub>();

            IPais pais = factory.Instantiate<IPais>();

            this.GarantirTipoDoObjeto<PaisStub>(pais);
        }

        /// <summary>
        /// Quando adicionamos dois mapeamentos para a mesma interface
        /// o primeiro é removido automaticamente.
        /// </summary>
        [Test]
        public void AdicionarMapeamento_Sobreescrever()
        {
            var factory = new GtFactory();

            factory.AddMapping<IPais, PaisStub>();
            factory.AddMapping<IPais, PaisStub2>();

            IPais pais = factory.Instantiate<IPais>();

            this.GarantirTipoDoObjeto<PaisStub2>(pais);
        }

        /// <summary>
        /// Quando chamamos o método LimparMapeamento para uma interface específica
        /// a fábrica remove o mapeamento e retorna ao seu compartamento padrão.
        /// </summary>
        [Test]
        public void LimparMapeamento()
        {
            var factory = new GtFactory();

            factory.AddMapping<IPais, PaisStub>();
            factory.AddMapping<ICidade, CidadeStub>();

            // O Método LimparMapeamento remove o mapeamento do tipo especificado.
            factory.CleanMappings<IPais>();

            IPais pais = factory.Instantiate<IPais>();
            ICidade cidade = factory.Instantiate<ICidade>();

            this.GarantirTipoDoObjeto<Pais>(pais);
            this.GarantirTipoDoObjeto<CidadeStub>(cidade);
        }

        /// <summary>
        /// Quando chamamos o método LimparTodosMapeamentos todos os mapeamentos feitos
        /// são removidos.
        /// </summary>
        [Test]
        public void LimparTodosMapeamentos()
        {
            var factory = new GtFactory();

            factory.AddMapping<IPais, PaisStub>();
            factory.AddMapping<ICidade, CidadeStub>();

            factory.ClearAllMappings();

            IPais pais = factory.Instantiate<IPais>();
            ICidade cidade = factory.Instantiate<ICidade>();

            this.GarantirTipoDoObjeto<Pais>(pais);
            this.GarantirTipoDoObjeto<Cidade>(cidade);
        }

        /// <summary>
        /// Não é possível utilizar a fábrica para instanciar classes diretamente, 
        /// somente interfaces.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidType))]
        public void Instanciar_Classe()
        {
            var factory = new GtFactory();

            factory.Instantiate<Pais>();
        }

        /// <summary>
        /// Caso o tipo alvo padrão não seja encontrado, uma exceção do tipo 
        /// "TipoAlvoNaoEncontrado" é disparada.
        /// </summary>
        [Test]
        [ExpectedException(typeof(TargetTypeNotFound))]
        public void Instanciar_ClassePadraoNaoExiste()
        {
            var factory = new GtFactory();

            factory.Instantiate<IContrato>();
        }

        /// <summary>
        /// Não é possível adicionar um mapeamento de uma classe pra outra, só é possível
        /// adicionar de uma interface para uma classe.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidType))]
        public void AdicionarMapeamento_ClasseParaClasse()
        {
            var factory = new GtFactory();

            factory.AddMapping<Pais, PaisStub2>();
        }

        /// <summary>
        /// Não é possível adicionar um mapeamento de uma interface para outra, só é possível
        /// adicionar de uma interface para uma classe.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidType))]
        public void AdicionarMapeamento_InterfaceParaInterface()
        {
            var factory = new GtFactory();

            factory.AddMapping<IPais, IPaisStub>();
        }

        /// <summary>
        /// Não é possível limpar os mapeamentos de uma classe, só é possível limpar os
        /// mapeamentos de uma interface.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidType))]
        public void LimparMapeamento_Classe()
        {
            var factory = new GtFactory();

            factory.CleanMappings<Pais>();
        }
    }

    #region Stubs

    internal interface IPais { }

    internal interface ITituloEleitor { }

    internal class TituloEleitor : ITituloEleitor { }

    internal interface IPaisStub : IPais { }

    internal class Pais : IPais { }

    internal class PaisStub : IPais { }

    internal class PaisStub2 : Pais, IPais { }

    internal class Cidade : ICidade { }

    internal class CidadeStub : ICidade { }

    internal interface ICidade { }

    internal interface IContrato { }

    #endregion


}
