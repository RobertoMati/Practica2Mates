using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    //Funcion que se ejecuta para ver quien gana
    private bool VerificacionBlackjack(int puntosJugador, int puntosDealer)
    {
        //Si los puntos del jugador son iguales a 21
        if(puntosJugador == 21){
            //El mensaje ya declarado en PlayAgain lo cambiamos
            finalMessage.text = "Blackjack! Has ganado!";
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            return true;
        }
        //Si los puntos del dealer son iguales a 21
        else if(puntosDealer == 21){
            //El mensaje ya declarado en PlayAgain lo cambiamos
            finalMessage.text = "Blackjack del Dealer! Has perdido!";
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
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
        if(puntosJugador == 21 || puntosDealer > 21 || (puntosDealer >= 17 && puntosDealer < puntosJugador)){
            //El mensaje ya declarado en PlayAgain lo cambiamos
            finalMessage.text = "Has ganado! Tienes " + puntosJugador + " puntos y el Dealer tiene " + puntosDealer + " puntos";
            PantallaFinPartida();
        }
        //Si los puntos del dealer son iguales a 21
        else if(puntosJugador > 21 || puntosDealer == 21 || (puntosDealer >= 17 && puntosDealer > puntosJugador)){
            //El mensaje ya declarado en PlayAgain lo cambiamos
            finalMessage.text = "Has perdido! Tienes " + puntosJugador + " puntos y el Dealer tiene " + puntosDealer + " puntos";
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
        if (blackJack == false)
        { 
            VerificacionFinalPartida(puntosJugador,puntosDealer);
        }
        else
        {
            PantallaFinPartida();
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

        //Creamos distintas variables para los puntos del jugador y del dealer


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
        if(dealer.GetComponent<CardHand>().GetPoints() <= 16)
        {
            PushDealer();
        }
        else if(dealer.GetComponent<CardHand>().GetPoints() > 16)
        {
            finalMessage.text = "Has ganado! Tienes " + player.GetComponent<CardHand>().GetPoints() + " puntos y el Dealer tiene " + dealer.GetComponent<CardHand>().GetPoints() + " puntos";
            PantallaFinPartida();
        }
         
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
