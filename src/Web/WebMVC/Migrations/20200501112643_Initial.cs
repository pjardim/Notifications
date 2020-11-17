using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace WebMVC.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationEvent",
                columns: table => new
                {
                    ApplicationEventId = table.Column<Guid>(nullable: false),
                    ApplicationEventName = table.Column<string>(type: "varchar(100)", nullable: true),
                    Description = table.Column<string>(type: "varchar(100)", nullable: true),
                    NotificationTemplateId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationEvent", x => x.ApplicationEventId);
                });

            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    ChannelId = table.Column<Guid>(nullable: false),
                    ChannelName = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.ChannelId);
                });

            migrationBuilder.CreateTable(
                name: "Subscriber",
                columns: table => new
                {
                    SubscriberPartyId = table.Column<string>(type: "varchar(100)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriber", x => x.SubscriberPartyId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriberGroup",
                columns: table => new
                {
                    SubscriberGroupId = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberGroup", x => x.SubscriberGroupId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationEventParameter",
                columns: table => new
                {
                    ApplicationEventParameterId = table.Column<Guid>(nullable: false),
                    ApplicationEventId = table.Column<Guid>(nullable: false),
                    ParameterName = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationEventParameter", x => x.ApplicationEventParameterId);
                    table.ForeignKey(
                        name: "FK_ApplicationEventParameter_ApplicationEvent_ApplicationEventId",
                        column: x => x.ApplicationEventId,
                        principalTable: "ApplicationEvent",
                        principalColumn: "ApplicationEventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriberFilter",
                columns: table => new
                {
                    SubscriberFilterId = table.Column<Guid>(nullable: false),
                    ApplicationEventId = table.Column<Guid>(nullable: false),
                    FilterType = table.Column<string>(type: "varchar(100)", nullable: true),
                    FilterValue = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberFilter", x => x.SubscriberFilterId);
                    table.ForeignKey(
                        name: "FK_SubscriberFilter_ApplicationEvent_ApplicationEventId",
                        column: x => x.ApplicationEventId,
                        principalTable: "ApplicationEvent",
                        principalColumn: "ApplicationEventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationEventChannel",
                columns: table => new
                {
                    ApplicationEventId = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false),
                    DelayedSendMinutes = table.Column<int>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    RequireAcknowledgement = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationEventChannel", x => new { x.ChannelId, x.ApplicationEventId });
                    table.ForeignKey(
                        name: "FK_ApplicationEventChannel_ApplicationEvent_ApplicationEventId",
                        column: x => x.ApplicationEventId,
                        principalTable: "ApplicationEvent",
                        principalColumn: "ApplicationEventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationEventChannel_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationEventChannelTemplate",
                columns: table => new
                {
                    ApplicationEventId = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false),
                    Format = table.Column<string>(type: "varchar(100)", nullable: false),
                    Encoding = table.Column<string>(type: "varchar(100)", nullable: false),
                    Subject = table.Column<string>(type: "varchar(100)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationEventChannelTemplate", x => new { x.ChannelId, x.ApplicationEventId });
                    table.ForeignKey(
                        name: "FK_ApplicationEventChannelTemplate_ApplicationEvent_ApplicationEventId",
                        column: x => x.ApplicationEventId,
                        principalTable: "ApplicationEvent",
                        principalColumn: "ApplicationEventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationEventChannelTemplate_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MailBoxItem",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(nullable: false),
                    RecipientPartyId = table.Column<string>(type: "varchar(100)", nullable: false),
                    SenderPartyIds = table.Column<string>(type: "varchar(100)", nullable: true),
                    DirectEmail = table.Column<bool>(nullable: false),
                    Subject = table.Column<string>(type: "varchar(100)", nullable: true),
                    Body = table.Column<string>(type: "varchar(max)", nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    RequireAcknowledged = table.Column<bool>(nullable: false),
                    Acknowledged = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Excluded = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailBoxItem", x => new { x.MessageId, x.RecipientPartyId });
                    table.ForeignKey(
                        name: "FK_MailBoxItem_Subscriber_RecipientPartyId",
                        column: x => x.RecipientPartyId,
                        principalTable: "Subscriber",
                        principalColumn: "SubscriberPartyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriberApplicationEvent",
                columns: table => new
                {
                    SubscriberPartyId = table.Column<string>(type: "varchar(100)", nullable: false),
                    ApplicationEventId = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberApplicationEvent", x => new { x.SubscriberPartyId, x.ApplicationEventId });
                    table.ForeignKey(
                        name: "FK_SubscriberApplicationEvent_ApplicationEvent_ApplicationEventId",
                        column: x => x.ApplicationEventId,
                        principalTable: "ApplicationEvent",
                        principalColumn: "ApplicationEventId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubscriberApplicationEvent_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubscriberApplicationEvent_Subscriber_SubscriberPartyId",
                        column: x => x.SubscriberPartyId,
                        principalTable: "Subscriber",
                        principalColumn: "SubscriberPartyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriberGroupSubscriber",
                columns: table => new
                {
                    SubscriberPartyId = table.Column<string>(type: "varchar(100)", nullable: false),
                    SubscriberGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberGroupSubscriber", x => new { x.SubscriberPartyId, x.SubscriberGroupId });
                    table.ForeignKey(
                        name: "FK_SubscriberGroupSubscriber_SubscriberGroup_SubscriberGroupId",
                        column: x => x.SubscriberGroupId,
                        principalTable: "SubscriberGroup",
                        principalColumn: "SubscriberGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubscriberGroupSubscriber_Subscriber_SubscriberPartyId",
                        column: x => x.SubscriberPartyId,
                        principalTable: "Subscriber",
                        principalColumn: "SubscriberPartyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEventChannel_ApplicationEventId",
                table: "ApplicationEventChannel",
                column: "ApplicationEventId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEventChannelTemplate_ApplicationEventId",
                table: "ApplicationEventChannelTemplate",
                column: "ApplicationEventId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEventParameter_ApplicationEventId",
                table: "ApplicationEventParameter",
                column: "ApplicationEventId");

            migrationBuilder.CreateIndex(
                name: "IX_MailBoxItem_RecipientPartyId",
                table: "MailBoxItem",
                column: "RecipientPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberApplicationEvent_ApplicationEventId",
                table: "SubscriberApplicationEvent",
                column: "ApplicationEventId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberApplicationEvent_ChannelId",
                table: "SubscriberApplicationEvent",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberFilter_ApplicationEventId",
                table: "SubscriberFilter",
                column: "ApplicationEventId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberGroupSubscriber_SubscriberGroupId",
                table: "SubscriberGroupSubscriber",
                column: "SubscriberGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationEventChannel");

            migrationBuilder.DropTable(
                name: "ApplicationEventChannelTemplate");

            migrationBuilder.DropTable(
                name: "ApplicationEventParameter");

            migrationBuilder.DropTable(
                name: "MailBoxItem");

            migrationBuilder.DropTable(
                name: "SubscriberApplicationEvent");

            migrationBuilder.DropTable(
                name: "SubscriberFilter");

            migrationBuilder.DropTable(
                name: "SubscriberGroupSubscriber");

            migrationBuilder.DropTable(
                name: "Channel");

            migrationBuilder.DropTable(
                name: "ApplicationEvent");

            migrationBuilder.DropTable(
                name: "SubscriberGroup");

            migrationBuilder.DropTable(
                name: "Subscriber");
        }
    }
}
