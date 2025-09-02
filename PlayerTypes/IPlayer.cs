    
    //KRAV 2:
    //1: Strategy Pattern
    //2: Vi har byggt vårt Strategy Pattern med hjälp av två olika typer av Players, InteractivePlayer
    //och ComputerPlayer, som både implementerar ett gemensamt gränssnitt kallat IPlayer. Syftet med detta 
    //gränsnitt är att ge oss en standard för alla spelares beslutstyper i spelet, oavsett om de är styrda av en människa eller AI. 
    //När vi skapar ett Player-objekt bestämmer vi vilken typ av beslut som ska användas, och det här valet kan ändras 
    //under programmets gång. 
    //3: Det betyder att om vi kör vår kod och tilldelar en ny typ av Player till spelaren, kan vi enkelt 
    //växla mellan att styra spelaren själva och låta AI ta över. Det är designat att Player klassen kan få olika beteenden 
    //utan att behöva ändra i själva klassen, eftersom beslutslogiken sköts av IPlayer-implementationen 


public interface IPlayer 
{
    string Name {get; set;}
    int Score {get; set;}
    Categories AvailableCategories { get; set; } // varje spelare håller sina kategorier
    bool HasAvailableCategories();
    string[] SelectDiceToHold(Dice dice);
    public string DecideBestCategory(Dice dice, bool[] holdDice);

}



