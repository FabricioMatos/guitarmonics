using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Guitarmonics.GameLib.Test.ControllerTests
{
    [TestFixture]
    public class GtTuneControllerTest
    {
        [Test]
        public void GtTuneControllerConstructor()
        {
        }

        //Instanciar controler ativa AudioListener (integracao com AudioListener)

        //Nota reconhecida (nota e distancia da afinação - positiva ou negativa) => Mockiar AudioListener

        //Mudanças só sao percebidas apos X milisegundos (penso inicialmente em X = 300) => Evitar flicker

        //Deve exibir o volume do que é tocado (considerar o volume da frequencia FFT de maior volume)

    }
}
