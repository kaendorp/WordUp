using UnityEngine;
using System.Collections;

public class CharManager : MonoBehaviour {


    private PlatformerCharacter2D character;
    [System.NonSerialized] PlayerRope playerRope;
    Rigidbody2D rigidbody;

    public GameObject nextParent;

    void Start()
    {
        character = GetComponent<PlatformerCharacter2D>();
    }

	void Awake(){
        character = GetComponent<PlatformerCharacter2D>();
        playerRope = gameObject.GetComponent<PlayerRope>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	void OnEnterRope(GameObject rope){

        character.anim.SetBool("Swinging", true);
        
        if((Input.GetAxisRaw("Vertical") != 1 && playerRope.enabled == false) || playerRope.segmentHashTable.ContainsValue(rope))
			return;
		
		playerRope.RegisterSegment(rope);
		

		if(transform.parent == null || transform.parent.position.y < rope.transform.position.y){
            playerRope.SetUpController(0.0f, 0.0f, 0.0f, true, false, false, true);
			
			if(transform.parent == null)
            {
				playerRope.previousParent = rope;
			}
            else
            {
				playerRope.previousParent = transform.parent.gameObject;   
			}

			transform.parent = rope.transform;
			playerRope.SetXOffset(0.0f);
            character.enabled = false;
			playerRope.enabled = true;
		}
	}
	
	void OnExitRope(GameObject rope){

        character.anim.SetBool("Swinging", false);
        rigidbody.gravityScale = 3f;

        if (playerRope.enabled == false)
			return;
		
		playerRope.UnregisterSegment(rope);
		
		if(transform.parent == rope.transform){
			transform.parent = playerRope.previousParent.transform;

			int tempValue = 0;
            
            foreach(DictionaryEntry segment in playerRope.segmentHashTable)
            {
                GameObject i = (GameObject) segment.Value;

                if((i.GetComponent<RopeSegment>().segmentNumber) > tempValue)
                {
                    nextParent = (GameObject) segment.Value;
                }
                if(transform.parent != (i.transform))
                {
                    tempValue = i.GetComponent<RopeSegment>().segmentNumber;
                }
            }
            if (nextParent)
            {
                playerRope.previousParent = nextParent;
            }
            else
            {
                playerRope.previousParent = transform.parent.gameObject;
            }
		}
		
		if(playerRope.segmentHashTable.Count == 0){
            character.jump = true;
            character.moveVelocity = 0;
			transform.parent = null;
			character.enabled = true;
			playerRope.enabled = false;
		}
	}

}
