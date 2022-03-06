using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicGates;

namespace LogicGates.UnitTests
{
    internal class ANDGateTest
    {
        [Test]
        public void AndGettingX0()
        {
            var andGate = new ANDGate();

            InputField input1 = new InputField();
            InputField input2 = new InputField();

            andGate.ConnectWith(input1);
            andGate.ConnectWith(input2);
            var result = andGate.Output;


            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void AndGettingX0X1()
        {
            var andGate = new ANDGate();

            InputField input1 = new InputField();
            InputField input2 = new InputField();

            input1.ChangeLogicalState();

            andGate.ConnectWith(input1);
            andGate.ConnectWith(input2);
            var result = andGate.Output;


            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void AndGettingX1()
        {
            var andGate = new ANDGate();

            InputField input1 = new InputField();
            InputField input2 = new InputField();
            input1.ChangeLogicalState();
            input2.ChangeLogicalState();

            andGate.ConnectWith(input1);
            andGate.ConnectWith(input2);
            var result = andGate.Output;


            Assert.That(result, Is.EqualTo(true));
        }
    }
}
