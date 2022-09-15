﻿// <auto-generated />
using System;
using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DatabaseFramework.Migrations
{
    [DbContext(typeof(AccountsDbContext))]
    partial class AccountsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CommunAxiom.Accounts.Models.AccountType", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("Code")
                    .HasColumnType("text");

                b.HasKey("Id");

                b.ToTable("AccountTypes", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.AppClaim", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<int>("AppNamespaceId")
                    .HasColumnType("integer");

                b.Property<string>("ClaimName")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<string>("Description")
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("AppNamespaceId");

                b.ToTable("AppClaim", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.AppClaimAssignment", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<int>("AppClaimId")
                    .HasColumnType("integer");

                b.Property<int>("ApplicationTypeId")
                    .HasColumnType("integer");

                b.Property<string>("AssignmentTags")
                    .HasColumnType("text");

                b.Property<string>("Value")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("AppClaimId");

                b.HasIndex("ApplicationTypeId");

                b.ToTable("AppClaimAssignment", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Application", b =>
            {
                b.Property<string>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("text");

                b.Property<string>("ClientId")
                    .HasMaxLength(100)
                    .HasColumnType("character varying(100)");

                b.Property<string>("ClientSecret")
                    .HasColumnType("text");

                b.Property<string>("ConcurrencyToken")
                    .IsConcurrencyToken()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)");

                b.Property<string>("ConsentType")
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)");

                b.Property<bool>("Deleted")
                    .HasColumnType("boolean");

                b.Property<DateTime?>("DeletedDate")
                    .HasColumnType("timestamp with time zone");

                b.Property<string>("DisplayName")
                    .HasColumnType("text");

                b.Property<string>("DisplayNames")
                    .HasColumnType("text");

                b.Property<string>("Permissions")
                    .HasColumnType("text");

                b.Property<string>("PostLogoutRedirectUris")
                    .HasColumnType("text");

                b.Property<string>("Properties")
                    .HasColumnType("text");

                b.Property<string>("RedirectUris")
                    .HasColumnType("text");

                b.Property<string>("Requirements")
                    .HasColumnType("text");

                b.Property<string>("Type")
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)");

                b.HasKey("Id");

                b.HasIndex("ClientId")
                    .IsUnique();

                b.ToTable("OpenIddictApplications", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.ApplicationType", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("Name")
                    .HasColumnType("text");

                b.HasKey("Id");

                b.ToTable("ApplicationTypes", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.ApplicationTypeMap", b =>
            {
                b.Property<string>("ApplicationId")
                    .HasColumnType("text");

                b.Property<int>("ApplicationTypeId")
                    .HasColumnType("integer");

                b.HasKey("ApplicationId");

                b.HasIndex("ApplicationTypeId");

                b.ToTable("ApplicationTypeMaps", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.AppNamespace", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("Description")
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("Id");

                b.ToTable("AppNamespace", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Authorization", b =>
            {
                b.Property<string>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("text");

                b.Property<string>("ApplicationId")
                    .HasColumnType("text");

                b.Property<string>("ConcurrencyToken")
                    .IsConcurrencyToken()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)");

                b.Property<DateTime?>("CreationDate")
                    .HasColumnType("timestamp with time zone");

                b.Property<string>("Properties")
                    .HasColumnType("text");

                b.Property<string>("Scopes")
                    .HasColumnType("text");

                b.Property<string>("Status")
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)");

                b.Property<string>("Subject")
                    .HasMaxLength(400)
                    .HasColumnType("character varying(400)");

                b.Property<string>("Type")
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)");

                b.HasKey("Id");

                b.HasIndex("ApplicationId", "Status", "Subject", "Type");

                b.ToTable("OpenIddictAuthorizations", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Contact", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<int>("CreationStatusId")
                    .HasColumnType("integer");

                b.Property<string>("PrimaryAccountId")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<string>("UserId")
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("CreationStatusId");

                b.HasIndex("PrimaryAccountId");

                b.HasIndex("UserId");

                b.ToTable("Contacts", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.ContactMethodType", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("Name")
                    .HasColumnType("text");

                b.HasKey("Id");

                b.ToTable("ContactMethodType", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.ContactRequest", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<int>("ContactId")
                    .HasColumnType("integer");

                b.Property<int>("ContactStatusId")
                    .HasColumnType("integer");

                b.Property<DateTime>("DateSent")
                    .HasColumnType("timestamp with time zone");

                b.Property<int>("IdProviderId")
                    .HasColumnType("integer");

                b.Property<int>("NotificationId")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.HasIndex("ContactId");

                b.HasIndex("ContactStatusId");

                b.HasIndex("IdProviderId");

                b.HasIndex("NotificationId");

                b.ToTable("ContactRequests", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.CreationStatus", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("Name")
                    .HasColumnType("text");

                b.HasKey("Id");

                b.ToTable("CreationStatus", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Group", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<byte[]>("GroupPicture")
                    .HasColumnType("bytea");

                b.Property<string>("Name")
                    .HasColumnType("text");

                b.Property<string>("OwnerId")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("OwnerId");

                b.ToTable("Groups", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.GroupMember", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<int>("GroupId")
                    .HasColumnType("integer");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("GroupId");

                b.HasIndex("UserId");

                b.ToTable("GroupMembers", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.GroupMemberRole", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<int>("GroupMemberId")
                    .HasColumnType("integer");

                b.Property<int>("GroupRoleId")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.HasIndex("GroupMemberId");

                b.HasIndex("GroupRoleId");

                b.ToTable("GroupMemberRole", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.GroupRole", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("Name")
                    .HasColumnType("text");

                b.HasKey("Id");

                b.ToTable("GroupRole", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.IdProvider", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("Name")
                    .HasColumnType("text");

                b.HasKey("Id");

                b.ToTable("IdProvider", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Notification", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<int>("ContactId")
                    .HasColumnType("integer");

                b.Property<int>("ContactMethodTypeId")
                    .HasColumnType("integer");

                b.Property<string>("Message")
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("ContactId");

                b.HasIndex("ContactMethodTypeId");

                b.ToTable("Notifications", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Scope", b =>
            {
                b.Property<string>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("text");

                b.Property<string>("ConcurrencyToken")
                    .IsConcurrencyToken()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)");

                b.Property<string>("Description")
                    .HasColumnType("text");

                b.Property<string>("Descriptions")
                    .HasColumnType("text");

                b.Property<string>("DisplayName")
                    .HasColumnType("text");

                b.Property<string>("DisplayNames")
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .HasMaxLength(200)
                    .HasColumnType("character varying(200)");

                b.Property<string>("Properties")
                    .HasColumnType("text");

                b.Property<string>("Resources")
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("Name")
                    .IsUnique();

                b.ToTable("OpenIddictScopes", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Token", b =>
            {
                b.Property<string>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("text");

                b.Property<string>("ApplicationId")
                    .HasColumnType("text");

                b.Property<string>("AuthorizationId")
                    .HasColumnType("text");

                b.Property<string>("ConcurrencyToken")
                    .IsConcurrencyToken()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)");

                b.Property<DateTime?>("CreationDate")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTime?>("ExpirationDate")
                    .HasColumnType("timestamp with time zone");

                b.Property<string>("Payload")
                    .HasColumnType("text");

                b.Property<string>("Properties")
                    .HasColumnType("text");

                b.Property<DateTime?>("RedemptionDate")
                    .HasColumnType("timestamp with time zone");

                b.Property<string>("ReferenceId")
                    .HasMaxLength(100)
                    .HasColumnType("character varying(100)");

                b.Property<string>("Status")
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)");

                b.Property<string>("Subject")
                    .HasMaxLength(400)
                    .HasColumnType("character varying(400)");

                b.Property<string>("Type")
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)");

                b.HasKey("Id");

                b.HasIndex("AuthorizationId");

                b.HasIndex("ReferenceId")
                    .IsUnique();

                b.HasIndex("ApplicationId", "Status", "Subject", "Type");

                b.ToTable("OpenIddictTokens", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.User", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("text");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("integer");

                b.Property<int>("AccountTypeId")
                    .HasColumnType("integer");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("text");

                b.Property<string>("Email")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("boolean");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("boolean");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("timestamp with time zone");

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<string>("PasswordHash")
                    .HasColumnType("text");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("text");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("boolean");

                b.Property<byte[]>("ProfilePicture")
                    .HasColumnType("bytea");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("text");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("boolean");

                b.Property<string>("UserName")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.HasKey("Id");

                b.HasIndex("AccountTypeId");

                b.HasIndex("NormalizedEmail")
                    .HasDatabaseName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex");

                b.ToTable("AspNetUsers", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.UserApplicationMap", b =>
            {
                b.Property<string>("ApplicationId")
                    .HasColumnType("text");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("ApplicationId");

                b.HasIndex("UserId");

                b.ToTable("UserApplicationMap", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("text");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex");

                b.ToTable("AspNetRoles", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("text");

                b.Property<string>("ClaimValue")
                    .HasColumnType("text");

                b.Property<string>("RoleId")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("text");

                b.Property<string>("ClaimValue")
                    .HasColumnType("text");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("text");

                b.Property<string>("ProviderKey")
                    .HasColumnType("text");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("text");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("text");

                b.Property<string>("RoleId")
                    .HasColumnType("text");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("text");

                b.Property<string>("LoginProvider")
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .HasColumnType("text");

                b.Property<string>("Value")
                    .HasColumnType("text");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens", (string)null);
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.AppClaim", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.AppNamespace", "AppNamespace")
                    .WithMany("AppClaims")
                    .HasForeignKey("AppNamespaceId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("AppNamespace");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.AppClaimAssignment", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.AppClaim", "AppClaim")
                    .WithMany()
                    .HasForeignKey("AppClaimId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.ApplicationType", "ApplicationType")
                    .WithMany()
                    .HasForeignKey("ApplicationTypeId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("AppClaim");

                b.Navigation("ApplicationType");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.ApplicationTypeMap", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.Application", "Application")
                    .WithMany("ApplicationTypeMaps")
                    .HasForeignKey("ApplicationId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.ApplicationType", "ApplicationType")
                    .WithMany()
                    .HasForeignKey("ApplicationTypeId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Application");

                b.Navigation("ApplicationType");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Authorization", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.Application", "Application")
                    .WithMany("Authorizations")
                    .HasForeignKey("ApplicationId");

                b.Navigation("Application");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Contact", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.CreationStatus", "CreationStatus")
                    .WithMany()
                    .HasForeignKey("CreationStatusId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.User", "PrimaryAccount")
                    .WithMany()
                    .HasForeignKey("PrimaryAccountId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId");

                b.Navigation("CreationStatus");

                b.Navigation("PrimaryAccount");

                b.Navigation("User");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.ContactRequest", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.Contact", "Contact")
                    .WithMany()
                    .HasForeignKey("ContactId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.CreationStatus", "ContactStatus")
                    .WithMany()
                    .HasForeignKey("ContactStatusId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.IdProvider", "IdProvider")
                    .WithMany()
                    .HasForeignKey("IdProviderId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.Notification", "Notification")
                    .WithMany()
                    .HasForeignKey("NotificationId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Contact");

                b.Navigation("ContactStatus");

                b.Navigation("IdProvider");

                b.Navigation("Notification");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Group", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.User", "Owner")
                    .WithMany()
                    .HasForeignKey("OwnerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Owner");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.GroupMember", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.Group", "Group")
                    .WithMany()
                    .HasForeignKey("GroupId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Group");

                b.Navigation("User");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.GroupMemberRole", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.GroupMember", "GroupMember")
                    .WithMany()
                    .HasForeignKey("GroupMemberId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.GroupRole", "GroupRole")
                    .WithMany()
                    .HasForeignKey("GroupRoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("GroupMember");

                b.Navigation("GroupRole");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Notification", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.Contact", "Contact")
                    .WithMany()
                    .HasForeignKey("ContactId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.ContactMethodType", "ContactMethodType")
                    .WithMany()
                    .HasForeignKey("ContactMethodTypeId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Contact");

                b.Navigation("ContactMethodType");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Token", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.Application", "Application")
                    .WithMany("Tokens")
                    .HasForeignKey("ApplicationId");

                b.HasOne("CommunAxiom.Accounts.Models.Authorization", "Authorization")
                    .WithMany("Tokens")
                    .HasForeignKey("AuthorizationId");

                b.Navigation("Application");

                b.Navigation("Authorization");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.User", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.AccountType", "AccountType")
                    .WithMany()
                    .HasForeignKey("AccountTypeId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("AccountType");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.UserApplicationMap", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.Application", "Application")
                    .WithMany("UserApplicationMaps")
                    .HasForeignKey("ApplicationId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Application");

                b.Navigation("User");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CommunAxiom.Accounts.Models.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("CommunAxiom.Accounts.Models.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Application", b =>
            {
                b.Navigation("ApplicationTypeMaps");

                b.Navigation("Authorizations");

                b.Navigation("Tokens");

                b.Navigation("UserApplicationMaps");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.AppNamespace", b =>
            {
                b.Navigation("AppClaims");
            });

            modelBuilder.Entity("CommunAxiom.Accounts.Models.Authorization", b =>
            {
                b.Navigation("Tokens");
            });
#pragma warning restore 612, 618
        }
    }
}
