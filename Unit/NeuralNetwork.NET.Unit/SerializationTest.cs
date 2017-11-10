﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworkNET.Networks.Activations;
using NeuralNetworkNET.Networks.Implementations;
using NeuralNetworkNET.Networks.PublicAPIs;

namespace NeuralNetworkNET.Unit
{
    /// <summary>
    /// Test class for the serialization methods
    /// </summary>
    [TestClass]
    [TestCategory(nameof(SerializationTest))]
    public class SerializationTest
    {
        [TestMethod]
        public void BinarySerialize()
        {
            NeuralNetwork network = NeuralNetwork.NewRandom(NetworkLayer.Inputs(5), NetworkLayer.FullyConnected(8, ActivationFunctionType.Sigmoid), NetworkLayer.FullyConnected(4, ActivationFunctionType.Sigmoid));
            float[] data = network.Serialize();
            NeuralNetwork copy = NeuralNetwork.Deserialize(data, NetworkLayer.Inputs(5), NetworkLayer.FullyConnected(8, ActivationFunctionType.Sigmoid), NetworkLayer.FullyConnected(4, ActivationFunctionType.Sigmoid));
            Assert.IsTrue(copy.Equals(network));
        }

        [TestMethod]
        public void BinaryDeserialize()
        {
            NeuralNetwork network = NeuralNetwork.NewRandom(NetworkLayer.Inputs(5), NetworkLayer.FullyConnected(8, ActivationFunctionType.Sigmoid), NetworkLayer.FullyConnected(4, ActivationFunctionType.Sigmoid));
            float[] data = network.Serialize();
            Assert.ThrowsException<InvalidOperationException>(() => NeuralNetwork.Deserialize(data, NetworkLayer.Inputs(5), NetworkLayer.FullyConnected(7, ActivationFunctionType.Sigmoid), NetworkLayer.FullyConnected(4, ActivationFunctionType.Sigmoid)));
        }

        [TestMethod]
        public void JsonSerialize()
        {
            NeuralNetwork network = NeuralNetwork.NewRandom(NetworkLayer.Inputs(5), NetworkLayer.FullyConnected(8, ActivationFunctionType.Sigmoid), NetworkLayer.FullyConnected(4, ActivationFunctionType.Sigmoid));
            String json = network.SerializeAsJSON();
            INeuralNetwork copy = NeuralNetworkDeserializer.TryDeserialize(json);
            Assert.IsTrue(copy != null);
            Assert.IsTrue(copy.Equals(network));
            String faulted = json.Replace("8", "7");
            copy = NeuralNetworkDeserializer.TryDeserialize(faulted);
            Assert.IsTrue(copy == null);
        }
    }
}
