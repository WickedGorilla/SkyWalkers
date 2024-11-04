using Game.Validation.ValidationActions;
using Infrastructure.Network.Request.Base;

namespace Game.Validation
{
    public class ValidationCoinsRequest : NetworkRequest
    {
        public IValidationAction[] ValidationActions;
    }
}