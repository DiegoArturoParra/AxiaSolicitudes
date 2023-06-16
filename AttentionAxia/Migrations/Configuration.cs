namespace AttentionAxia.Migrations
{
    using AttentionAxia.Core;
    using AttentionAxia.Core.Data;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<AxiaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AxiaContext context)
        {
            AxiaDBInitializer.Seed(context);
            base.Seed(context);
        }
    }
}
