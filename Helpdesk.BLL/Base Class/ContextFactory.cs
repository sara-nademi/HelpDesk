using System.Data.Objects;


namespace Helpdesk.BLL
{
    public static class ContextFactory
    {
        private static ObjectContext _objectContext;

        public static void DisposeContext()
        {
            if (_objectContext != null)
            {
                _objectContext.Dispose();
                _objectContext = null;
            }
        }

        public static ObjectContext GetContext()
        {
            if (_objectContext == null)
                _objectContext = new DAL.Helpdesk90Entities();

            return _objectContext;
        }
    }
}
