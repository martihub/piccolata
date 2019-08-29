
namespace BehaviorDesigner.Runtime.Tasks
{
    public class RotateSlot : Action
    {
        public SharedGameObjectList slots;
        public SharedGameObject clickedSlot;
        public SharedBool isOnlyClickedSlot;
        public SharedBool isFront;
        public SharedFloat waitDuration;
        public SharedBool headShake;

        public override void OnStart()
        {
            if (!isOnlyClickedSlot.Value)
            {
                foreach (var item in slots.Value)
                {
                    item.GetComponent<MatchPart>().ChangeSprite(isFront.Value, waitDuration.Value, headShake.Value);
                    item.GetComponent<MatchPart>().isClicked = false;
                }
            }
            else
            {
                clickedSlot.Value.GetComponent<MatchPart>().ChangeSprite(isFront.Value, waitDuration.Value, headShake.Value);
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            slots = null;
            clickedSlot = null;
            isOnlyClickedSlot = false;
            isFront = false;
            waitDuration = 1;
        }
    }

}
