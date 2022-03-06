using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicGates;

namespace LogicGates.UnitTests
{
    internal class NANDGateTest
    {
        [Test]
        public void NANDGateTakeArgument00()
        {
            var andGate = new ANDGate();
            var notGate = new NOTGate();

            InputField input1 = new InputField();
            InputField input2 = new InputField();

            andGate.ConnectWith(input1);
            andGate.ConnectWith(input2);
            notGate.ConnectWith(andGate);
            var result = notGate.Output;


            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void NANDGateTakeArgument01()
        {
            var andGate = new ANDGate();
            var notGate = new NOTGate();

            InputField input1 = new InputField();
            InputField input2 = new InputField();
            input1.ChangeLogicalState();

            andGate.ConnectWith(input1);
            andGate.ConnectWith(input2);
            notGate.ConnectWith(andGate);
            var result = notGate.Output;


            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void NANDGateTakeArgument11()
        {
            var andGate = new ANDGate();
            var notGate = new NOTGate();

            InputField input1 = new InputField();
            InputField input2 = new InputField();
            input1.ChangeLogicalState();
            input2.ChangeLogicalState();

            andGate.ConnectWith(input1);
            andGate.ConnectWith(input2);
            notGate.ConnectWith(andGate);
            var result = notGate.Output;


            Assert.That(result, Is.EqualTo(false));
        }
    }
}
