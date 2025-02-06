using UnityEngine;

public class Flocker : Kinematic
{
    public bool avoiding = false;
    public GameObject MainBurb;
    BlendSteering mySteering;
    PrioritySteering myPriority;
    Kinematic[] MyBurbs;

    public void Start()
    {
        LookWhereGoing myRotateType = new LookWhereGoing();
        myRotateType.character = this;
        Separation separation = new Separation();
        separation.character = this;
        Arrive arrive = new Arrive();
        arrive.character = this;
        arrive.target = MainBurb;

        GameObject[] Burbs = GameObject.FindGameObjectsWithTag("burb");
        MyBurbs = new Kinematic[Burbs.Length - 1];
        int x = 0;
        for(int i = 0; i < Burbs.Length; i++)
        {
            if (Burbs[i] == this)
            {
                continue;
            }
            //x++;
            MyBurbs[x] = Burbs[i].GetComponent<Kinematic>();
        }
        separation.targets = MyBurbs;

        mySteering = new BlendSteering();
        mySteering.behaviors = new BehaviorAndWeight[3];
        mySteering.behaviors[0] = new BehaviorAndWeight();
        mySteering.behaviors[0].behavior = separation;
        mySteering.behaviors[0].weight = 1f;
        mySteering.behaviors[1] = new BehaviorAndWeight();
        mySteering.behaviors[1].behavior = arrive;
        mySteering.behaviors[1].weight = .5f;
        mySteering.behaviors[2] = new BehaviorAndWeight();
        mySteering.behaviors[2].behavior = myRotateType;
        mySteering.behaviors[2].weight = 2f;

        ObstacleAvoid avoid = new ObstacleAvoid();
        avoid.character = this;
        avoid.target = MainBurb;
        avoid.flee = true;
        BlendSteering HighPriority = new BlendSteering();
        HighPriority.behaviors = new BehaviorAndWeight[1];
        HighPriority.behaviors[0] = new BehaviorAndWeight();
        HighPriority.behaviors[0].behavior = avoid;
        HighPriority.behaviors[0].weight = 1f;

        myPriority = new PrioritySteering();
        myPriority.groups = new BlendSteering[2];
        myPriority.groups[0] = new BlendSteering();
        myPriority.groups[0] = HighPriority;
        myPriority.groups[1] = new BlendSteering();
        myPriority.groups[1] = mySteering;
    }

    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        if (!avoiding)
        {
            steeringUpdate = mySteering.getSteering();
        }
        else
        {
            steeringUpdate = myPriority.getSteering();
        }
        base.Update();
    }
}
