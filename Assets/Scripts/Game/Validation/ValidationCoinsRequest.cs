using Infrastructure.Network.Request.Base;
using Newtonsoft.Json;

namespace Game.Validation
{
    public class ValidationCoinsRequest : ServerRequest
    {
        public ValidationAction[] ValidationActions;

        public ValidationCoinsRequest(IPlayerActionData[] actionsDatas)
        {
            ValidationActions = new ValidationAction[actionsDatas.Length];
            
            int index = 0;
            foreach (IPlayerActionData data in actionsDatas)
            {
                ValidationActions[index] = new ValidationAction(data);
                index++;
            }
        }
    }
    
    public class ValidationAction
    {
        public ValidationType ActionType;
        public string JsonData;

        public ValidationAction(IPlayerActionData actionData)
        {
            ActionType = actionData.ActionType;
            JsonData = JsonConvert.SerializeObject(actionData.GetObjectForJson());
        }
        
        public enum ValidationType
        {
            TapCoins = 0,
            ActivatePlayPass = 1,
            ActivateBoost = 2,
            EndBoost = 3,
            TapCoinsWithBoost = 4
        }
    }
}