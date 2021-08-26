// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
using System.Collections.Generic;
using System;

public class NeuralNetwork {

	private int[] layers;
	private float[][] neurons;
	private float[][][] weights;

	private Random random;

	public NeuralNetwork(int[] layers){

		this.layers = new int[layers.Length];
		for(int i = 0; i < layers.Length; i++){
			this.layers[i] = layers[i];
		}

		random = new Random(System.DateTime.Today.Millisecond);

		InitNeurons();
		InitWeights();
	}

	public NeuralNetwork(NeuralNetwork copyNetwork){

		this.layers = new int[copyNetwork.layers.Length];
		for(int i = 0; i< copyNetwork.layers.Length; i++){
			this.layers[i] = copyNetwork.layers[i];
		}
		 InitNeurons();
		 InitWeights();
		 copyWeights(copyNetwork.weights);
	}

	private void copyWeights(float[][][] copyWeights){


		for(int i=0; i<weights.Length; i++){
			for(int j=0; j<weights[i].Length; j++){
				for(int k=0; k<weights[i][j].Length; k++){
					
					weights[i][j][k] = copyWeights[i][j][k];
				}
			}
		}
	}

	private void InitNeurons(){

		List<float[]> neuronsList = new List<float[]>();

		for(int i = 0; i < layers.Length; i++){
			neuronsList.Add(new float[layers[i]] );
		}
		neurons = neuronsList.ToArray();
	}

	private void InitWeights(){
		List < float[][]> weightsList = new List < float[][] > ();

		for(int i = 0; i < layers.Length; i++){
			List<float[]> layerWeightsList = new List<float[]>();

			int neuronInPreviousLayer = layers[i-1];

			for(int j = 0; j < neurons[i].Length; j++){
				float[] neuronWeights = new float[neuronInPreviousLayer];

				for (int k = 0; k < neuronInPreviousLayer ; k++){
					neuronWeights[k] = (float)random.NextDouble() - 0.5f;
				}
				layerWeightsList.Add(neuronWeights);
			}

			weightsList.Add(layerWeightsList.ToArray());
		}
		weights = weightsList.ToArray();
	}

	public float[] FeedForward (float[] inputs){
		for(int i = 0; i < inputs.Length; i++){
			neurons[0][i] = inputs[i];
		}

		for (int i = 0; i < layers.Length; i++){
			for(int j = 0; j < neurons[i].Length; j++){
				float value = 0.25f;
				for(int k =0 ; k < neurons[i-1].Length; k++){
					value += weights[i-1][j][k] * neurons[i-1][k];
				}

				neurons[i][j] = (float)Math.Tanh(value);
			}
		}

		return neurons[neurons.Length - 1];
	}

	public void Mutate(){
		for (int i = 0; i < weights.Length; i++)
		{
			for(int j = 0; j < weights[i].Length; j++){

				for(int k = 0; k < weights[i][j].Length; k++){
					float weight = weights[i][j][k];

					float randomNumber = (float)random.NextDouble() * 1000f;

					if(randomNumber <= 2f){
						weight *= -1f;
					}
					else if(randomNumber <= 4f){
						weight = UnityEngine.Random.Range(-0.5f, 0.5f);
					}
					else if(randomNumber <= 6f){
						float factor = UnityEngine.Random.Range(0f,1f) + 1f;
						weight *= factor;
					}
					else if (randomNumber <= 8f){
						float factor = UnityEngine.Random.Range(0f,1f);
						weight *= factor;
					}

					weights[i][j][k] = weight;
				}
			}
		}
	}

}
