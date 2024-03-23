
namespace MathGame.DataAccess
{
    public interface IContext
    {
        MathGameDbContext Instance { get; }
    }
    public class Context : IContext
    {
        private readonly MathGameDbContext context;
        public Context(MathGameDbContext mContext)
        {
            context = mContext;
        }

        public MathGameDbContext Instance => context;
    }
}
