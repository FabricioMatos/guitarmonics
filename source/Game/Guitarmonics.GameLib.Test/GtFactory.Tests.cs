using NUnit.Framework;
using Guitarmonics.GameLib;

namespace Guitarmonics.GameLib.Testes
{
    [TestFixture]
    public class GtFactoryTests
    {
        #region [ Setup ]

        /// <summary>
        /// Verifica se o objeto � do tipo informado.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="pObjeto">Inst�ncia do objeto</param>
        private void GarantirTipoDoObjeto<T>(object pObjeto)
        {
            Assert.IsNotNull(pObjeto);
            Assert.IsInstanceOfType(typeof(T), pObjeto);
        }

        #endregion

        /// <summary>
        /// Quando pedimos � f�brica para criar a inst�ncia de uma interface, 
        /// ela vai retornar a inst�ncia da classe concreta de mesmo nome que 
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
        /// Ao adicionar um mapeamento � F�brica alteramos o comportamento padr�o da mesma,
        /// sendo que toda vez que for solicitada uma inst�ncia do tipo ela retornar� uma inst�ncia
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
        /// o primeiro � removido automaticamente.
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
        /// Quando chamamos o m�todo LimparMapeamento para uma interface espec�fica
        /// a f�brica remove o mapeamento e retorna ao seu compartamento padr�o.
        /// </summary>
        [Test]
        public void LimparMapeamento()
        {
            var factory = new GtFactory();

            factory.AddMapping<IPais, PaisStub>();
            factory.AddMapping<ICidade, CidadeStub>();

            // O M�todo LimparMapeamento remove o mapeamento do tipo especificado.
            factory.CleanMappings<IPais>();

            IPais pais = factory.Instantiate<IPais>();
            ICidade cidade = factory.Instantiate<ICidade>();

            this.GarantirTipoDoObjeto<Pais>(pais);
            this.GarantirTipoDoObjeto<CidadeStub>(cidade);
        }

        /// <summary>
        /// Quando chamamos o m�todo LimparTodosMapeamentos todos os mapeamentos feitos
        /// s�o removidos.
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
        /// N�o � poss�vel utilizar a f�brica para instanciar classes diretamente, 
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
        /// Caso o tipo alvo padr�o n�o seja encontrado, uma exce��o do tipo 
        /// "TipoAlvoNaoEncontrado" � disparada.
        /// </summary>
        [Test]
        [ExpectedException(typeof(TargetTypeNotFound))]
        public void Instanciar_ClassePadraoNaoExiste()
        {
            var factory = new GtFactory();

            factory.Instantiate<IContrato>();
        }

        /// <summary>
        /// N�o � poss�vel adicionar um mapeamento de uma classe pra outra, s� � poss�vel
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
        /// N�o � poss�vel adicionar um mapeamento de uma interface para outra, s� � poss�vel
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
        /// N�o � poss�vel limpar os mapeamentos de uma classe, s� � poss�vel limpar os
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
