#pragma strict

var targetObject : GameObject;
var waitTime:float = 1.0;
private var lineRenderer : LineRenderer;

function Start() {
    lineRenderer = GetComponent(LineRenderer);
}

function Update () {

	if(waitTime == 5){
	
	    lineRenderer.SetPosition(0,this.transform.localPosition);
	    
	    for(var i:int=1;i<4;i++)
	    {
	        var pos = Vector3.Lerp(this.transform.localPosition,targetObject.transform.localPosition,i/5.0f);
	        
	        pos.x += Random.Range(-0.6f,0.7f);
	        pos.y += Random.Range(-0.3f,0.5f);
	        
	        lineRenderer.SetPosition(i,pos);
	    }
	    
	    lineRenderer.SetPosition(4,targetObject.transform.localPosition);
    }
    waitTime += 1;
    
    if(waitTime >=6){
    	waitTime = 0;
    	}
}