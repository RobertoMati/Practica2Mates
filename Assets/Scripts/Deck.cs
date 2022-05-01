using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using System.Collections.Generic;

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
    public Text probMessage2;
    public Text probMessage3;

    public int[] values = new int[52];
    int cardIndex = 0; 

    private bool estadoCarta;

//------------------------------------------------------------
    //Declaración Extra
    public Text BancaValor;

    //Valor inicial de la banca
    private int Banca = 1000;

    //Mutiplos para el valor de la apuesta
    private int Apuesta = 10;
    public bool apuesta = false;

    //Botones Apuesta
    public Button AumentarA;
    public Button DisminuirA;

    public Text textApuesta;

    public Toggle toggle;



       
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

    //Funcion que se ejecuta para ver quien gana
    private bool VerificacionBlackjack(int puntosJugador, int puntosDealer)
    {
        //Si los puntos del jugador son iguales a 21
        if(puntosJugador == 21){
            //El mensaje ya declarado en PlayAgain lo cambiamos
            finalMessage.text = "Blackjack! Has ganado!";
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            PantallaFinPartida();
            ApuestaGanada();
            return true;
        }
        //Si los puntos del dealer son iguales a 21
        else if(puntosDealer == 21){
            //El mensaje ya declarado en PlayAgain lo cambiamos
            finalMessage.text = "Blackjack del Dealer! Has perdido!";
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            PantallaFinPartida();
            ApuestaPerdida();
            return true;
        }
        else{
            return false;
        }

    }

    //Funcion que se ejecuta para acabar el juego
    private void VerificacionFinalPartida(int puntosJugador, int puntosDealer)
    {
        //Si los puntos del jugador son iguales a 21 o los puntos del dealer son mayores a 21 o si el dealer tiene 17 o más pero menos que el jugador
        if(puntosJugador == 21 || puntosDealer > 21){
            //El mensaje ya declarado en PlayAgain lo cambiamos
            finalMessage.text = "Has ganado! Tienes " + puntosJugador + " puntos y el Dealer tiene " + puntosDealer + " puntos";
            PantallaFinPartida();
            ApuestaGanada();
        }
        //Si los puntos del dealer son iguales a 21
        else if((puntosJugador > 21 && puntosDealer<=21) || puntosJugador > 21 || puntosDealer == 21 ){
            //El mensaje ya declarado en PlayAgain lo cambiamos
            finalMessage.text = "Has perdido! Tienes " + puntosJugador + " puntos y el Dealer tiene " + puntosDealer + " puntos";
            PantallaFinPartida();
            ApuestaPerdida();
        }
        else if(puntosJugador ==21 && puntosDealer==21){
            //El mensaje ya declarado en PlayAgain lo cambiamos
            finalMessage.text = "Empate! Tienes " + puntosJugador + " puntos y el Dealer tiene " + puntosDealer + " puntos";
            PantallaFinPartida();
        }
    }

     private void PantallaFinPartida()
    {
        stickButton.interactable = false;
        hitButton.interactable = false;
    }


//----------------------------------------------------------------------------------------------------------------------
    void StartGame()
    {
        //Creamos las variables para los puntos y el blackjack
        int puntosJugador = 0;
        int puntosDealer = 0;
        bool blackJack = false;
        
        for (int i = 0; i < 2; i++)
        {
            PushDealer();
            PushPlayer();

            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
            */
        }
        //Igualamos los puntos del jugador y del dealer a los puntos que conseguimos en CardHand
        puntosJugador = player.GetComponent<CardHand>().GetPoints();
        puntosDealer = dealer.GetComponent<CardHand>().GetPoints();
        blackJack = VerificacionBlackjack(puntosJugador, puntosDealer);
        
        //Si el blackjack cambia a true
        if (!blackJack)
        { 
            VerificacionBlackjack(puntosJugador,puntosDealer);
        }

        TextoApuesta(Apuesta);
        TextoBanca(Banca);

    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */

    //------------------------------------------------
        //Caso 1: probabilidad de que el dealer tenga más puntuación que el jugador con una carta oculta
        float probabilidad;
        int valorTotalDealer = dealer.GetComponent<CardHand>().GetPoints();
        int valorTotalJugador = player.GetComponent<CardHand>().GetPoints();
        int valorOcultoDealer = dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().value;
        //Debug.Log("Valor total del dealer: " + valorOcultoDealer);
        int casosFavorables;

        //Con esto damos a entender que acabamos de empezar la partida, por tanto, el dealer puede tener una carta oculta
        if(dealer.GetComponent<CardHand>().cards.Count == 2 && player.GetComponent<CardHand>().cards.Count == 2)
        {
            estadoCarta = true;
        }
        else
        {
            estadoCarta = false;
        }

        //El dealer tiene una carta oculta
        if(estadoCarta){
            int PuntosDealerTotales = valorTotalDealer - valorOcultoDealer;
            //Debug.Log("Puntos del dealer: " + PuntosDealerTotales);
            //13 es el número de tipos de cartas que hay en el juego
            casosFavorables = 13 - valorTotalJugador + PuntosDealerTotales;
            //la f del 13 es para forzar que se trate como un float. Sin la f da o 0 o 1
            probabilidad = casosFavorables / 13f;
            float probabilidadRound = Mathf.Round(probabilidad * 100);
            //Si probabilidadRound es mayor a 100, se le asigna 100
            if(probabilidadRound > 100)
            {
                probabilidadRound = 100;
            }
            //Si probabilidadRound es menor a 0, se le asigna 0
            if(probabilidadRound < 0)
            {
                probabilidadRound = 0;
            }

            probMessage.text = "- Probabilidad de que el dealer tenga más puntuación que el jugador con una carta oculta: " + probabilidadRound + "%";
        }
    //------------------------------------------------

    //------------------------------------------------
        //Caso 2: Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
            //Para calcular la probabilidad, tenemos que ver los casos en los que se pasa de 21 sumándole, uno a uno, cada carta posible.
            //Para ello, al valor máximo que tenemos, le restamos el valor total de puntos del jugador.
            //Con esto, al numero de cartas que hay (13) le restamos lo obtenido anteriormente.
            int valorRestado = 21 - valorTotalJugador;
            casosFavorables = 13 - valorRestado;


            //Otro método
            List<string> listaValores = new List<string>();
            for(int i = 0; i < 13; i++)
            {
                int x = valorTotalJugador + (i+1);
                Debug.Log("Valor de la carta: " + valorTotalJugador);
                listaValores.Add(x.ToString());
                Debug.Log(listaValores[i]);
            }
            



            //Dividimos el valor obtenido entrero entre el número de cartas que hay (13)
            probabilidad = casosFavorables / 13f;
            float probabilidadRound2 = Mathf.Round(probabilidad * 100);
            probMessage2.text = "- Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta: " + probabilidadRound2 + "%";
            
        
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

        if (dealer.GetComponent<CardHand>().cards.Count == 2)
        {
          dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true); 
        }
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */  
         VerificacionFinalPartida(player.GetComponent<CardHand>().GetPoints(), dealer.GetComponent<CardHand>().GetPoints());    

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        if (dealer.GetComponent<CardHand>().cards.Count == 2)
        {
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
        }

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */   
        if(dealer.GetComponent<CardHand>().GetPoints() < 17)
        {
            PushDealer();
        }
        else if(dealer.GetComponent<CardHand>().GetPoints() >16)
        {
            /*finalMessage.text = "Has ganado! Tienes " + player.GetComponent<CardHand>().GetPoints() + " puntos y el Dealer tiene " + dealer.GetComponent<CardHand>().GetPoints() + " puntos";
            PantallaFinPartida();
            ApuestaGanada();*/
            VerificacionFinalPartida(player.GetComponent<CardHand>().GetPoints(), dealer.GetComponent<CardHand>().GetPoints());
        }
        VerificacionFinalPartida(player.GetComponent<CardHand>().GetPoints(), dealer.GetComponent<CardHand>().GetPoints());
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
        DesactivarToggle();
        apuesta = false;
    }

    //----------------------------------------------------------
    //EXTRA
    //Funcion para aumentar la apuesta
    private void AumentarApuesta()
    {
        if (Apuesta + 10 <= Banca)
        {
            Apuesta += 10;
            textApuesta.text = Apuesta.ToString();
            TextoApuesta(Apuesta);
        }
    }

    //Funcion para disminuir la apuesta
    private void DisminuirApuesta()
    {
        if (Apuesta - 10 >= 0)
        {
            Apuesta -= 10;
            textApuesta.text = Apuesta.ToString();
            TextoApuesta(Apuesta);
        }
    }

    private void TextoApuesta(int apuestaActualizada)
    {
        textApuesta.text = apuestaActualizada.ToString();
        
    }

    public void GuardarApuesta()
    {
        if (apuesta)
        {
            apuesta = false;
        }
        else
        {
            apuesta = true;
        }
    }

    private void ApuestaGanada()
    {
        if (apuesta)
        {
            Banca += (Apuesta*2);
            TextoBanca(Banca);
        }
    }

    private void ApuestaPerdida()
    {
        if (apuesta)
        {
            Banca -= Apuesta;
            TextoBanca(Banca);

        }
    }


    private void TextoBanca(int valorBancaActualizado)
    {
        BancaValor.text = "" + valorBancaActualizado;
    }

    private void DesactivarToggle()
    {
        toggle.isOn = false;
    }
    
}
