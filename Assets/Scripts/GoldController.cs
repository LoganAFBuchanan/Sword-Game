using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{

    private int goldValue;

    public Sprite lowValSprite;
    public Sprite midValSprite;
    public Sprite highValSprite;

    private SpriteRenderer spriteRender;

    public enum ValTier
    {
        low,
        mid,
        high
    };


    public ValTier tier;

    // Use this for initialization
    void Start()
    {
		spriteRender = GetComponent<SpriteRenderer>();
        switch (tier)
        {
            case ValTier.low:
                spriteRender.sprite = lowValSprite;
				goldValue = 10;
                break;
            case ValTier.mid:
                spriteRender.sprite = midValSprite;
				goldValue = 25;
                break;
            case ValTier.high:
                spriteRender.sprite = highValSprite;
				goldValue = 50;
                break;
            default:
                spriteRender.sprite = lowValSprite;
				goldValue = 10;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.layer == 11)
        {
			other.GetComponent<PlayerController>().stats.goldCount += goldValue;
			Destroy(this.gameObject);
		}
    }

	public void SetValTier(int tierInput){
		switch(tierInput){
            case 0:
				tier = ValTier.low;
                break;
            case 1:
				tier = ValTier.mid;
                break;
            case 2:
				tier = ValTier.high;
                break;
            default:
				tier = ValTier.low;
                break;
        }
	}
}
