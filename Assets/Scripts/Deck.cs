﻿using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
         //contador de cartas
        int cartas = 0;

        //recorremos todas las cartas de todos los palos
        for (int i=0; i <52; i++){
            //empezamos por la primera y vamos sumando de uno en uno
            cartas++;
            //si es menor que diez o diez
            if(cartas<=10){
                //si no es un as
                if(cartas!= 1){
                    //le atribuimos el valor a la carta
                    values[i]=cartas;
                }
                else{
                    //si es un as le asignamos el 11 que es "los puntos que vale"
                    values[i]=11;
                }
            }
            else{
                //si llegamos a 13 carta reiniciamos el contador
                if(cartas==13){
                    cartas=0;
                }
                //a partir de la carta 10 le asignamos el valor 10 que es los puntos que vale
                values[i]=10;
            }
        }
        
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */      
         //guarda la posición futura
        int next = 0;
        //guarda el valor auxiliar
        int valorAuxiliar = 0;
        //imagen sprite
        Sprite auxFace = null;

        //recorremos el array de las cartes
        for (int i=0; i <values.Length; i++)
        {
        //nos guardamos el número devuelto por el random
        next = Random.Range(0,52);
        //nos guardamos en la varaible el elemento que contenga values en la posición next(aleatorio)
        valorAuxiliar = values[next];
        //obtenemos la face
        auxFace = faces[next];


        //en la posicion aleatorio nos guardamos lo que haya en la posición del array
        values[next] = values[i];
        faces[next] = faces[i];
        
        //en la posición 0 por ejemplo nos guardamos lo que previamente hemos sacado
        values[i]= valorAuxiliar;
        faces[i] = auxFace;
        }
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
