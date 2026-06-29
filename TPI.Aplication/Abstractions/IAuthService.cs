using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Abstractions
{
    public interface IAuthService
    {
        AuthResponse SignUp(SignUpRequest request);
        AuthResponse SignIn(SignInRequest request);
    }
}
