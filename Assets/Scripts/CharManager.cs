using UnityEngine;
using System.Collections;

public class CharManager : MonoBehaviour {


    [System.NonSerialized] PlatformerCharacter2D platformerCharacter;
    [System.NonSerialized] Platformer2DUserControl platformerUserControl;
    [System.NonSerialized] PlayerRope playerRope;

    public GameObject nextParent;

	void Awake(){
        platformerCharacter = gameObject.GetComponent<PlatformerCharacter2D>();
        platformerUserControl = gameObject.GetComponent<Platformer2DUserControl>();
        playerRope = gameObject.GetComponent<PlayerRope>();
	}
	
	void OnEnterRope(GameObject rope){

		if((Input.GetAxisRaw("Vertical") != 1 && playerRope.enabled == false) || playerRope.segmentHashTable.ContainsValue(rope))
			return;
		
		playerRope.RegisterSegment(rope);
		
		if(transform.parent == null || transform.parent.position.y < rope.transform.position.y){
			playerRope.SetUpController(0, 0, 0, true, false, false, true);
			
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
			platformerCharacter.enabled = false;
			playerRope.enabled = true;
		}
	}
	
	void OnExitRope(GameObject rope){
		if (playerRope.enabled == false)
			return;
		
		playerRope.UnregisterSegment(rope);
		
		if(transform.parent == rope.transform){
			transform.parent = playerRope.previousParent.transform;
			//some setup
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
            platformerUserControl.jump = true;
            platformerCharacter.moveVelocity = 0;
			transform.parent = null;
			platformerCharacter.enabled = true;
			playerRope.enabled = false;
		}
	}

}
