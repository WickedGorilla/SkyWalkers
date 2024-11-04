using Game.Validation.ValidationActions;
using Infrastructure.Network.Request.Base;

namespace Game.Validation
{
    public class ValidationCoinsRequest : ServerRequest
    {
        public IValidationAction[] ValidationActions;

        public ValidationCoinsRequest(IValidationAction[] validationActions) 
            => ValidationActions = validationActions;
    }
}