using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Topic
{
    sport = 0,
    family = 1,
    joke = 2

}

public class Conversation : Interactable
{
    public Sprite SucceededSprite;
    public Sprite FailedSprite;
    public Topic Topic1;
    public Topic Topic2;

    public UnityEvent TopicSelect;
    public static Dictionary<string, UnityEvent> Conversations = new Dictionary<string, UnityEvent>();

    public string ConversationName = "Default";
    [TextArea] public string ConversationDescription = "Do the thing.";
    public bool succeeded;
    private SpriteRenderer icon;


    [SerializeField] [Range(0, 2)] private int statUsed = 0;
    [SerializeField] [Range(0, 2)] private int difficultyTier;
    private int difficultyModifier = 0;
    private Color[] colours = new Color[3];
    private float probability;
    private float percentChance;
    private int notorietyYield;


    protected override void Start()
    {
        base.Start();


        TopicSelect = new UnityEvent();
        TopicSelect.AddListener(TriggerConversation);
        Conversations.Add(ConversationName, TopicSelect);

        icon = base.visualiser.GetComponent<SpriteRenderer>();

        //TODO
        //load sprite for correct topic types if player has that information
        
    }

    // Update is called once per frame
    protected override void Interaction()
    {
        probability = (difficultyModifier + CharacterInfo.Stats[statUsed]) / 10.0f;
        ConversationConfirm.ConversationInteraction.Invoke(ConversationName, ConversationDescription, (float)System.Math.Round(probability * 100.0f, 2), statUsed);
        Debug.Log("Interacted with Conversation: " + ConversationName);
    }

    private void TriggerConversation()
    {
        float roll = Random.Range(0.0f, 1.0f);
        if (roll <= probability)
        {
            succeeded = true;
            icon.sprite = SucceededSprite;
            icon.color = colours[0];
            NotorietyMeter.NotorietyChange.Invoke(notorietyYield);
        }
        else
        {
            succeeded = false;
            icon.sprite = FailedSprite;
            icon.color = colours[2];
        }
        base.useable = false;
        CharacterInfo.TimeAdvanceEvent.Invoke();
    }
}