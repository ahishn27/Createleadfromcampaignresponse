using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;

namespace Campaignresponsetolead
{
    public class crToLead : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            //Tracing service for debugging
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(IServiceProvider));
            tracingService.Trace("Tracing service invoked");

            //Get context
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            int step = 0;


            if (context.InputParameters.Contains("Target") & context.InputParameters["Target"] is Entity)
            {
                Entity campaignresponse = (Entity)context.InputParameters["Target"];

                if (campaignresponse.LogicalName != "campaignresponse")
                    return;

                try
                {
                    step = 1;

                    string subject = "";
                    string firstName = "";
                    string lastName = "";
                    string emailid = "";
                    string phone = "";
                    Guid LeadId = Guid.Empty;


                    IOrganizationServiceFactory organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                    IOrganizationService organizationService = organizationServiceFactory.CreateOrganizationService(context.UserId);

                    if (campaignresponse.Attributes.Contains("subject"))
                        subject = campaignresponse["subject"].ToString();
                    tracingService.Trace("Subject: " + subject);

                    if (campaignresponse.Attributes.Contains("firstname"))
                        firstName = campaignresponse["firstname"].ToString();
                    tracingService.Trace("FirstName: " + firstName);
                    
                    if (campaignresponse.Attributes.Contains("lastname"))
                        lastName = campaignresponse["lastname"].ToString();
                    tracingService.Trace("LastName: " + lastName);

                    if (campaignresponse.Attributes.Contains("emailaddress"))
                        emailid = campaignresponse["emailaddress"].ToString();
                    tracingService.Trace("Email: " + emailid);

                    step = 2;

                    Entity lead = new Entity("Lead");
                    tracingService.Trace("Lead Invoked");

                    lead["subject"] = subject;
                    lead["firstname"] = firstName;
                    lead["lastname"] = lastName;
                    lead["emailaddress1"] = emailid;
                    lead["relatedobjectid"] = new EntityReference("lead", campaignresponse.Id);
                    tracingService.Trace("Lead Created");
                }
                catch (Exception e)
                {
                    tracingService.Trace("{0}", e.ToString());
                    throw;
                }
            }
        }
    }
}
