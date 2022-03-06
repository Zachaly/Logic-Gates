using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LogicGates;

namespace LogicGates.UnitTests
{
    internal class ORGateTest
    {
        CustomGate NAND()
        {
            
            InputField inp1 = new InputField();
            InputField inp2 = new InputField();

            GateSchema schema = new GateSchema();
            ANDGate andGate = new ANDGate();
            NOTGate notGate = new NOTGate();
            OutputField outputField = new OutputField();

            schema.Gates = new List<LogicGate>();

            andGate.ConnectWith(inp1);
            andGate.ConnectWith(inp2);
            notGate.ConnectWith(andGate);
            
            outputField.ConnectWith(notGate);

            schema.Gates.AddRange(new LogicGate[] { inp1, inp2, andGate, notGate, outputField });
            schema.Output = outputField;

            return new CustomGate("NAND", schema.TestClone());
        }

        [Test]
        public void ORGateTakeArgument00()
        {
            InputField input1 = new InputField();
            InputField input2 = new InputField();

            CustomGate nand1 = NAND();
            CustomGate nand2 = NAND();

            nand1.ConnectWith(input1);
            nand1.ConnectWith(input1);
            nand2.ConnectWith(input2);
            nand2.ConnectWith(input2);
            CustomGate nand3 = NAND();

            nand3.ConnectWith(nand1);
            nand3.ConnectWith(nand2);

            var result = nand3.Output;

            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void ORGateTakeArgument01()
        {
            InputField input1 = new InputField();
            InputField input2 = new InputField();
            input1.ChangeLogicalState();

            CustomGate nand1 = NAND();
            CustomGate nand2 = NAND();

            nand1.ConnectWith(input1);
            nand1.ConnectWith(input1);
            nand2.ConnectWith(input2);
            nand2.ConnectWith(input2);
            CustomGate nand3 = NAND();

            nand3.ConnectWith(nand1);
            nand3.ConnectWith(nand2);

            var result = nand3.Output;

            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public void ORGateTakeArgument11()
        {
            InputField input1 = new InputField();
            InputField input2 = new InputField();
            input1.ChangeLogicalState();
            input2.ChangeLogicalState();

            CustomGate nand1 = NAND();
            CustomGate nand2 = NAND();

            nand1.ConnectWith(input1);
            nand1.ConnectWith(input1);
            nand2.ConnectWith(input2);
            nand2.ConnectWith(input2);
            CustomGate nand3 = NAND();

            nand3.ConnectWith(nand1);
            nand3.ConnectWith(nand2);

            var result = nand3.Output;

            Assert.That(result, Is.EqualTo(true));
        }
    }
}
