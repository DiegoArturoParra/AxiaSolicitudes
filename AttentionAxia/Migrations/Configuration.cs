namespace AttentionAxia.Migrations
{
    using AttentionAxia.Core;
    using AttentionAxia.Core.Data;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AxiaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AxiaContext context)
        {
            if (context.TablaRoles.Count() == 0 && context.TablaUsuarios.Count() == 0)
            {
                AxiaDBInitializer.Seed(context);

            }
            base.Seed(context);
        }
    }
}
