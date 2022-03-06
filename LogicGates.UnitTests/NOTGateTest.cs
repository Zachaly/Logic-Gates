using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicGates;
using System.Threading.Tasks;

namespace LogicGates.UnitTests
{
    internal class NOTGateTest
    {
        [Test]
        public void NOTGateTakesArgumentFalse()
        {
            var notGate = new NOTGate();

            InputField input1 = new InputField();

            notGate.ConnectWith(input1);
            var result = notGate.Output;


            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void NOTGateTakesArgumentTrue()
        {
            var notGate = new NOTGate();

            InputField input1 = new InputField();
            input1.ChangeLogicalState();
            notGate.ConnectWith(input1);
            var result = notGate.Output;


            Assert.That(result, Is.EqualTo(false));
        }
        
    }
}
