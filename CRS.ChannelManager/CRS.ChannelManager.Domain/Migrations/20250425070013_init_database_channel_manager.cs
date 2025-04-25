using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CRS.ChannelManager.Domain.Migrations
{
    /// <inheritdoc />
    public partial class initdatabasechannelmanager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "channel_manager");

            migrationBuilder.CreateTable(
                name: "account",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hotelid = table.Column<long>(name: "hotel_id", type: "bigint", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    taxcode = table.Column<string>(name: "tax_code", type: "character varying(500)", maxLength: 500, nullable: true),
                    taxname = table.Column<string>(name: "tax_name", type: "character varying(500)", maxLength: 500, nullable: true),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    phone = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    email = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    marketsegmentid = table.Column<long>(name: "market_segment_id", type: "bigint", nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "channel",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "country",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    status = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hotel",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hotelid = table.Column<string>(name: "hotel_id", type: "character varying(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    shortname = table.Column<string>(name: "short_name", type: "character varying(255)", maxLength: 255, nullable: false),
                    fullname = table.Column<string>(name: "full_name", type: "character varying(512)", maxLength: 512, nullable: false),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    thumbnailimage = table.Column<string>(name: "thumbnail_image", type: "text", nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hotel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identification_type",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    documentconfig = table.Column<string>(name: "document_config", type: "text", maxLength: 2147483647, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identification_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "market_segment",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_market_segment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "package",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_package", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "package_plan",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hotelid = table.Column<string>(name: "hotel_id", type: "character varying(50)", maxLength: 50, nullable: true),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_package_plan", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    hotelid = table.Column<string>(name: "hotel_id", type: "character varying(50)", maxLength: 50, nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rate_plan",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rate_plan", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "room_type",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hotelid = table.Column<string>(name: "hotel_id", type: "character varying(50)", maxLength: 50, nullable: false),
                    roomtypeid = table.Column<string>(name: "room_type_id", type: "character varying(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    totaladult = table.Column<int>(name: "total_adult", type: "integer", nullable: false),
                    totalchild = table.Column<int>(name: "total_child", type: "integer", nullable: false),
                    thumbnailimage = table.Column<string>(name: "thumbnail_image", type: "text", nullable: true),
                    roomsize = table.Column<int>(name: "room_size", type: "integer", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "salutation",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salutation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hotelid = table.Column<string>(name: "hotel_id", type: "character varying(50)", maxLength: 50, nullable: true),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sub_segment ",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    marketsegmentcode = table.Column<string>(name: "market_segment_code", type: "character varying(50)", maxLength: 50, nullable: false),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sub_segment ", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "travel_agent",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    synckey = table.Column<long>(name: "sync_key", type: "bigint", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_travel_agent", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "channel_room_type",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hotelid = table.Column<long>(name: "hotel_id", type: "bigint", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    displayrate = table.Column<long>(name: "display_rate", type: "bigint", nullable: true),
                    nameunaccent = table.Column<string>(name: "name_unaccent", type: "character varying(500)", maxLength: 500, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channel_room_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_channel_room_type_hotel_hotel_id",
                        column: x => x.hotelid,
                        principalSchema: "channel_manager",
                        principalTable: "hotel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "channel_mapping_room_type",
                schema: "channel_manager",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    channelroomtypeid = table.Column<long>(name: "channel_room_type_id", type: "bigint", nullable: false),
                    hotelid = table.Column<long>(name: "hotel_id", type: "bigint", nullable: false),
                    channelid = table.Column<long>(name: "channel_id", type: "bigint", nullable: false),
                    accountid = table.Column<long>(name: "account_id", type: "bigint", nullable: false),
                    productid = table.Column<long>(name: "product_id", type: "bigint", nullable: false),
                    packageplanid = table.Column<long>(name: "package_plan_id", type: "bigint", nullable: false),
                    roomtypeid = table.Column<long>(name: "room_type_id", type: "bigint", nullable: false),
                    status = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    createdby = table.Column<string>(name: "created_by", type: "varchar", maxLength: 255, nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp", nullable: false),
                    modifieddate = table.Column<DateTime>(name: "modified_date", type: "timestamp", nullable: true),
                    modifiedby = table.Column<string>(name: "modified_by", type: "character varying(255)", maxLength: 255, nullable: true),
                    deleted = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp", nullable: true),
                    deletedby = table.Column<string>(name: "deleted_by", type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channel_mapping_room_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_channel_mapping_room_type_account_account_id",
                        column: x => x.accountid,
                        principalSchema: "channel_manager",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_channel_mapping_room_type_channel_channel_id",
                        column: x => x.channelid,
                        principalSchema: "channel_manager",
                        principalTable: "channel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_channel_mapping_room_type_channel_room_type_channel_room_ty~",
                        column: x => x.channelroomtypeid,
                        principalSchema: "channel_manager",
                        principalTable: "channel_room_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_channel_mapping_room_type_hotel_hotel_id",
                        column: x => x.hotelid,
                        principalSchema: "channel_manager",
                        principalTable: "hotel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_channel_mapping_room_type_package_plan_package_plan_id",
                        column: x => x.packageplanid,
                        principalSchema: "channel_manager",
                        principalTable: "package_plan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_channel_mapping_room_type_product_product_id",
                        column: x => x.productid,
                        principalSchema: "channel_manager",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_channel_mapping_room_type_room_type_room_type_id",
                        column: x => x.roomtypeid,
                        principalSchema: "channel_manager",
                        principalTable: "room_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_channel_mapping_room_type_account_id",
                schema: "channel_manager",
                table: "channel_mapping_room_type",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_channel_mapping_room_type_channel_id",
                schema: "channel_manager",
                table: "channel_mapping_room_type",
                column: "channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_channel_mapping_room_type_channel_room_type_id",
                schema: "channel_manager",
                table: "channel_mapping_room_type",
                column: "channel_room_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_channel_mapping_room_type_hotel_id",
                schema: "channel_manager",
                table: "channel_mapping_room_type",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "IX_channel_mapping_room_type_package_plan_id",
                schema: "channel_manager",
                table: "channel_mapping_room_type",
                column: "package_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_channel_mapping_room_type_product_id",
                schema: "channel_manager",
                table: "channel_mapping_room_type",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_channel_mapping_room_type_room_type_id",
                schema: "channel_manager",
                table: "channel_mapping_room_type",
                column: "room_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_channel_room_type_code",
                schema: "channel_manager",
                table: "channel_room_type",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_channel_room_type_hotel_id",
                schema: "channel_manager",
                table: "channel_room_type",
                column: "hotel_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "channel_mapping_room_type",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "country",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "identification_type",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "market_segment",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "package",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "rate_plan",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "salutation",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "service",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "sub_segment ",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "travel_agent",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "account",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "channel",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "channel_room_type",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "package_plan",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "product",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "room_type",
                schema: "channel_manager");

            migrationBuilder.DropTable(
                name: "hotel",
                schema: "channel_manager");
        }
    }
}
