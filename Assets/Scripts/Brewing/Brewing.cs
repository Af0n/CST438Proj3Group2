using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brewing : MonoBehaviour
{
    [Tooltip("Brewing ticks. Starts counting once all ingredients are near")]
    public int processTime;
    [Tooltip("Set potion type")]
    public ItemType type;
    [Tooltip("Range from which to detect materials")]
    public float range;
    [Tooltip("Layer to look for items on")]
    public LayerMask layerMask;

    [Header("Unity Set Up")]
    [Tooltip("Recipe reference list")]
    public Recipes recipes;
    [Tooltip("Position for items to hover")]
    public Transform itemPos;
    [Tooltip("Item prefab to instantiate\nDO NOT CHANGE")]
    public GameObject prefab;
    [Tooltip("Potion sprite renderer")]
    public SpriteRenderer spriteRenderer;

    // main timing variable
    private int tickTimer;

    // numerical representation of potion type
    private int typeCode;
    
    private Recipe selectedRecipe;
    private Sprite selectPotSprite;
    private bool isBrewing;
    private Transform[] targetIngredients;

    public bool IsBrewing{
        get { return isBrewing;  }
    }

    private void OnEnable() {
        TickSystem.OnTick += Tick;
    }

    private void OnDisable() {
        TickSystem.OnTick -= Tick;
    }

    private void Awake() {
        SetPotion(type);
    }

    public void TryCycleRecipe(){
        // do not enable recipe swapping while brewing
        if(IsBrewing){
            return;
        }

        typeCode++;

        if(typeCode >= recipes.recipes.Length){
            typeCode = 0;
        }

        selectedRecipe = recipes.recipes[typeCode];

        SetPotion(selectedRecipe.type);
    }

    // sets variables properly
    public void SetPotion(ItemType _type){
        isBrewing = false;
        type = _type;
        typeCode = GetPotionTypeCode(type);

        if(typeCode == -1){
            type = ItemType.POTION_CALM;
        }

        typeCode = GetPotionTypeCode(type);
        selectedRecipe = recipes.recipes[typeCode];
        selectPotSprite = selectedRecipe.sprite;
        spriteRenderer.sprite = selectPotSprite;
    }
    
    public void Tick(){
        tickTimer --;
        tickTimer = Mathf.Max(0, tickTimer);

        if(isBrewing){
            // go in here if currently brewing

            if(tickTimer <= 0){
                tickTimer = processTime;
                EndBrewing();
                SpawnItem(type);
                if(selectedRecipe.containsBucket){
                    SpawnItem(ItemType.BUCKET);
                }
            }
            return;
        }

        // comes here if not brewing
        
        // don't do anything if recipe is not satisfied
        if(!IsRecipeSatisfied()){
            return;
        }

        StartBrewing();
    }

    private void StartBrewing(){
        isBrewing = true;
        tickTimer = processTime;

        TargetIngredients();
        GrabTargetIngredients();
    }

    private void GrabTargetIngredients(){
        foreach (Transform item in targetIngredients)
        {
            item.GetComponent<PickUp>().Pick(itemPos);
        }
    }

    private void TargetIngredients(){
        // getting all nearby items
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, range, layerMask);
        targetIngredients = new Transform[selectedRecipe.ingredients.Length];
        int index = 0;

        // loop through each ingredient in the selected recipe
        foreach (ItemType ingredient in selectedRecipe.ingredients)
        {
            // getting the first available item of that ingredient type
            foreach (Collider2D item in items)
            {
                ItemType testingType = item.GetComponent<Item>().Type;

                if(ingredient == testingType){ 
                    // sets this once, then breaks out of this loop
                    targetIngredients[index] = item.transform;
                    index++;
                    break;
                }
            }
        }
    }

    private void EndBrewing(){
        isBrewing = false;

        foreach (Transform item in targetIngredients)
        {
            Destroy(item.gameObject);
        }

        targetIngredients = null;
    }

    public void SpawnItem(ItemType t)
    {
        // position to spawn
        Vector2 pos = itemPos.position;
        // instantiation
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        // changing object to the proper type
        obj.GetComponent<Item>().ChangeItem(t);
    }

    private bool IsRecipeSatisfied(){
        // getting all nearby items
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, range, layerMask);

        // no items nearby
        if(items.Length == 0){
            return false;
        }

        // loop through each ingredient in the selected recipe
        foreach (ItemType ingredient in selectedRecipe.ingredients)
        {
            bool ingredientSatisfied = false;

            // checking if that item is present nearby
            foreach (Collider2D item in items)
            {
                ItemType testingType = item.GetComponent<Item>().Type;

                if(ingredient == testingType){ 
                    // sets this once, then breaks out of this loop
                    ingredientSatisfied = true;  
                    break;
                }

                // set to false for every item
                ingredientSatisfied = false;
            }

            // early return if a single ingredient is not satisfied
            if(!ingredientSatisfied){
                return false;
            }
        }

        // only ever reach here if all ingredients pass the satisfaction check
        return true;
    }

    public bool Mix(){
        return false;
    }

    private int GetPotionTypeCode(ItemType i){
        switch(i){
            case ItemType.POTION_CALM:
                return 0;
            case ItemType.POTION_HEAL:
                return 1;
            case ItemType.POTION_MANA:
                return 2;
            default:
                // should never happen
                return -1;
        }
    }
}
