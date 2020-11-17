using Subscribing.Domain;
using System;
using System.Linq;

namespace Subscribing.Infrastructure.SeedData
{
    public static class SubscriberDbContextSeed
    {
        public static void Initialize(SubscriberContext context)
        {
            context.Database.EnsureCreated();

            if (context.Subscribers.Any())
            {
                return;   // DB has been seeded
            }

            var applicationEvents = new ApplicationEvent[]
            {
                  new ApplicationEvent(Guid.NewGuid(), "CrewCommentCreated","Crew Comment"),

                  new ApplicationEvent(Guid.NewGuid(),"BackOfficeCommentCreated","Admin Comment"),
            };

            foreach (var app in applicationEvents)
            {
                context.ApplicationEvents.Add(app);
            }
            context.SaveChanges();

            var subscribers = new Subscriber[]
            {
                 new Subscriber("123","crewEmail@test.com","CrewName" ),

                 new Subscriber("456","adminEmail@test.com","AdminName"),

                 new Subscriber("737","jetblueAdmin@rainmaker.ie","Joshua" ),

                 new Subscriber("19893","jetblueAdmin@rainmaker.ie","CrewPay SystemAdmin"),
            };

            foreach (var sub in subscribers)
            {
                context.Subscribers.Add(sub);
            }
            context.SaveChanges();

            context.SubscriberGroups.AddRange(SubscriberGroup.List());

            context.SaveChanges();

            var subscriberFilters = new SubscriberFilter[]
            {
                    new SubscriberFilter(Guid.NewGuid(), applicationEvents.Single( s => s.ApplicationEventName == "BackOfficeCommentCreated").Id,
                      SubscriberFilterType.EventPayload.Name, SubscriberGroup.CrewMember.Name),

                    new SubscriberFilter(Guid.NewGuid(), applicationEvents.Single( s => s.ApplicationEventName == "CrewCommentCreated").Id,
                    SubscriberFilterType.PartyGroup.Name, SubscriberGroup.SystemAdmin.Name),

                    new SubscriberFilter(Guid.NewGuid(), applicationEvents.Single( s => s.ApplicationEventName == "CrewCommentCreated").Id,
                    SubscriberFilterType.PartyGroup.Name, SubscriberGroup.BackOffice.Name),
            };

            foreach (var subfilter in subscriberFilters)
            {
                context.SubscriberFilters.Add(subfilter);
            }
            context.SaveChanges();

            var subscriberGroupSubscribers = new SubscriberGroupSubscriber[]
            {
                new SubscriberGroupSubscriber(subscribers.Single( s => s.Name == "CrewName").SubscriberPartyId
                    , SubscriberGroup.CrewMember.Id),

                new SubscriberGroupSubscriber(subscribers.Single( s => s.Name == "AdminName").SubscriberPartyId
                    , SubscriberGroup.BackOffice.Id),

                new SubscriberGroupSubscriber(subscribers.Single( s => s.Name == "AdminName").SubscriberPartyId
                    , SubscriberGroup.SystemAdmin.Id),

                new SubscriberGroupSubscriber(subscribers.Single( s => s.Name == "Joshua").SubscriberPartyId
                    , SubscriberGroup.CrewMember.Id),

                new SubscriberGroupSubscriber(subscribers.Single( s => s.Name == "CrewPay SystemAdmin").SubscriberPartyId
                    , SubscriberGroup.SystemAdmin.Id),
                      
                new SubscriberGroupSubscriber(subscribers.Single( s => s.Name == "CrewPay SystemAdmin").SubscriberPartyId
                    , SubscriberGroup.BackOffice.Id),

            };

            foreach (var subscriberGroupSubscriber in subscriberGroupSubscribers)
            {
                context.SubscriberGroupSubscribers.Add(subscriberGroupSubscriber);
            }
            context.SaveChanges();

            var channels = new Channel[]
            {
                new Channel("Email"),
                new Channel("SMS"),
                new Channel("NotificationsApp"),
            };

            foreach (var channel in channels)
            {
                context.Channels.Add(channel);
            }
            context.SaveChanges();

            var subscriberApplicationEvents = new SubscriberApplicationEvent[]
            {
               new SubscriberApplicationEvent(subscribers.Single( s => s.Name == "AdminName").SubscriberPartyId,
                applicationEvents.Single( s => s.ApplicationEventName == "CrewCommentCreated").Id, channels.Single(c=> c.ChannelName == "Email").Id),

               new SubscriberApplicationEvent(subscribers.Single( s => s.Name == "CrewName").SubscriberPartyId,
                  applicationEvents.Single( s => s.ApplicationEventName == "BackOfficeCommentCreated").Id, channels.Single(c=> c.ChannelName == "Email").Id),

               new SubscriberApplicationEvent(subscribers.Single( s => s.Name == "CrewPay SystemAdmin").SubscriberPartyId,
                  applicationEvents.Single( s => s.ApplicationEventName == "CrewCommentCreated").Id, channels.Single(c=> c.ChannelName == "Email").Id),

               new SubscriberApplicationEvent(subscribers.Single( s => s.Name == "Joshua").SubscriberPartyId,
                  applicationEvents.Single( s => s.ApplicationEventName == "BackOfficeCommentCreated").Id, channels.Single(c=> c.ChannelName == "Email").Id),


            };

            foreach (var subscriberApplicationEvent in subscriberApplicationEvents)
            {
                context.SubscriberApplicationEvents.Add(subscriberApplicationEvent);
            }
            context.SaveChanges();

            var applicationEventChannels = new ApplicationEventChannel[]
            {
                new ApplicationEventChannel(applicationEvents.Single( s => s.ApplicationEventName == "CrewCommentCreated").Id,
                                      channels.Single( s => s.ChannelName == "Email").Id, 5 ,true, true),

                  new ApplicationEventChannel(applicationEvents.Single( s => s.ApplicationEventName == "BackOfficeCommentCreated").Id,
                                      channels.Single( s => s.ChannelName == "Email").Id, 1 ,true,false),
            };

            foreach (var applicationEventChannel in applicationEventChannels)
            {
                context.ApplicationEventChannels.Add(applicationEventChannel);
            }
            context.SaveChanges();

            var applicationEventParameters = new ApplicationEventParameter[]
            {
                new ApplicationEventParameter(Guid.NewGuid(),applicationEvents.Single( s => s.ApplicationEventName == "CrewCommentCreated").Id,
                                      "Comment","Comment"),

                new ApplicationEventParameter(Guid.NewGuid(),applicationEvents.Single( s => s.ApplicationEventName == "CrewCommentCreated").Id,
                                      "CrewPayReportDetails","CrewPayReportDetails"),

               new ApplicationEventParameter(Guid.NewGuid(),applicationEvents.Single( s => s.ApplicationEventName == "BackOfficeCommentCreated").Id,
                                      "Comment","Comment"),

               new ApplicationEventParameter(Guid.NewGuid(),applicationEvents.Single( s => s.ApplicationEventName == "BackOfficeCommentCreated").Id,
                                      "CrewPayReportDetails","CrewPayReportDetails"),
            };

            foreach (var applicationEventParameter in applicationEventParameters)
            {
                context.ApplicationEventParameters.Add(applicationEventParameter);
            }

            context.SaveChanges();

            var applicationEventChannelTemplates = new ApplicationEventChannelTemplate[]

           {
                new ApplicationEventChannelTemplate(applicationEvents.Single( s => s.ApplicationEventName == "BackOfficeCommentCreated").Id,
                channels.Single( s => s.ChannelName == "Email").Id, "","", "BackOfficeCommentCreated", @"<p>Dear Crew Member</p><p>[[Notifications]]<br></p><p><br></p><p>{{CrewPayReportId}}</p><p><br></p>"),

                  new ApplicationEventChannelTemplate(applicationEvents.Single( s => s.ApplicationEventName == "CrewCommentCreated").Id,
                channels.Single( s => s.ChannelName == "Email").Id, "","", "Crew Comment Created", @"<p>Dear Crew Member</p><p>[[Notifications]]</p><p><br></p><p>{{CrewPayReportId}}</p><p><br></p>"),
           };

            foreach (var applicationEventChannelTemplate in applicationEventChannelTemplates)
            {
                context.ApplicationEventChannelTemplates.Add(applicationEventChannelTemplate);
            }
            context.SaveChanges();
        }
    }
}