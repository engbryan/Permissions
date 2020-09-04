using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.Restier.Core;
using Microsoft.Restier.Core.Model;
using Microsoft.Restier.Core.Query;
using Microsoft.Restier.Core.Submit;
using Microsoft.Restier.EntityFramework;
using Restier;
using Restier.DataContext;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Northwind.API
{
    public class NorthwindApi : EntityFrameworkApi<NorthwindEntitiesContainer>
    {
        public NorthwindApi(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
            //As permissões podem ser registradas e poderão ser atualizadas a cada X minutos por outra rotina personalizada ou
            //pelo próprio serviço, armazenando os dados das permissões vindas do banco, em memória, para as próximas autorizacões.

            //A cada nova requisição, deverá ser obtida a Role do usuário corrente e checar as permissões registradas, e comparar
            //com os tipos de permissão registrados para a operação requisitada.

            //O sistema deve impedir que as requisições executem qualquer query em banco caso o usuário não tenha tal permissão,
            //mesmo que o resultado seja ocultado na resposta do servidor, de forma a previnir execução de queries maliciosas.

            //permissões gerais
            this.RegisterPermission(Role = "Administrator", Access = GrantAccessType.All);
            this.RegisterPermission(Role = "Manager", Access = GrantAccessType.Discover | GrantAccessType.Get);
            this.RegisterPermission(Role = "Receptionist", Access = GrantAccessType.None);
            this.RegisterPermission(Role = "Anonymous", Access = GrantAccessType.None);

            //permissões granulares
            this.RegisterPermission(Role = "Manager", Access = GrantAccessType.Discover | GrantAccessType.Get | GrantAccessType.Add | GrantAccessType.Set, On = "Hotel");
            this.RegisterPermission(Role = "Manager", Access = GrantAccessType.Discover | GrantAccessType.Get | GrantAccessType.Set, On = "HotelGroup");
            this.RegisterPermission(Role = "Manager", Access = GrantAccessType.Discover | GrantAccessType.Get, On = "Hotel.HotelGroupId"); //automaticamente também em "Hotel.HotelGroup" por ser uma FK

            //permissões granulares
            this.RegisterPermission(Role = "Receptionist", Access = GrantAccessType.Discover | GrantAccessType.Get, On = "Hotel" );
            this.RegisterPermission(Role = "Receptionist", Access = GrantAccessType.All, On = "Address" );
            this.RegisterPermission(Role = "Receptionist", Access = GrantAccessType.Set, On = "Hotel.Description");

            //permissões granulares
            this.RegisterPermission(Role = "Anonymous", Access = GrantAccessType.Discover | GrantAccessType.Get, On = new string[] { "Hotel", "Address" });
            this.RegisterPermission(Role = "Anonymous", Access = GrantAccessType.None, On = "Hotel.TrueReview");

        }

        ////Gets called after inserting an entity to the entity set Products.
        //protected void OnUpdatingHotel(Hotel entity)
        //{
        //}

        //// Gets called after inserting an entity to the entity set Products.
        //protected void OnUpdatedHotel(Hotel entity)
        //{
        //}

        //// Gets called after inserting an entity to the entity set Products.
        //protected void OnExecutingHotel(Hotel entity)
        //{
        //}

        //protected IQueryable<Hotel> OnFilterHotel(IQueryable<Hotel> entitySet)
        //{
        //    return entitySet.Where(s => s.Id % 1 == 0).AsQueryable();
        //}

    }

    public class CustomizedQueryAuthorizer : IQueryExpressionAuthorizer
    {
        public bool Authorize(QueryExpressionContext context)
        {
            return true;
        }

    }

    public class CustomizedPayloadValueConverter : ODataPayloadValueConverter
    {
        public override object ConvertToPayloadValue(object value, IEdmTypeReference edmTypeReference)
        {
            if (edmTypeReference != null)
            {
                if (value is string)
                {
                    var stringValue = (string)value;

                    // Make a single string value "Russell" converted to have additional suffix
                    if (stringValue == "Russell")
                    {
                        return stringValue + "Converter";
                    }
                }
            }

            return base.ConvertToPayloadValue(value, edmTypeReference);
        }
    }

    public class CustomizedModelBuilder : IModelBuilder
    {
        public static IModelBuilder InnerModelBuilder { get; set; }

        public async Task<IEdmModel> GetModelAsync(ModelContext context, CancellationToken cancellationToken)
        {
            IEdmModel model = null;

            // Call inner model builder to get a model to extend.
            if (InnerModelBuilder != null)
            {
                model = await InnerModelBuilder.GetModelAsync(context, cancellationToken);
            }

            InnerModelBuilder = this;

            // Do sth to extend the model such as add custom navigation property binding.
            var builder = new ODataConventionModelBuilder();
            builder.EntityType<Hotel>();

            return builder.GetEdmModel();

            return model;
        }
    }

    public class CustomizedValidator : IChangeSetItemValidator
    {
        // Add any customized validation into this method
        public Task ValidateChangeSetItemAsync(
            SubmitContext context,
            ChangeSetItem item,
            Collection<ChangeSetItemValidationResult> validationResults,
            CancellationToken cancellationToken)
        {
            return null;
        }
    }
}