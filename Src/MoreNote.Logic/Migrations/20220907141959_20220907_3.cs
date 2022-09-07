using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class _20220907_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_records",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ip = table.Column<string>(type: "text", nullable: true),
                    x_real_ip = table.Column<string>(type: "text", nullable: true),
                    x_forwarded_for = table.Column<string>(type: "text", nullable: true),
                    referrer = table.Column<string>(type: "text", nullable: true),
                    request_header = table.Column<string>(type: "text", nullable: true),
                    access_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    unix_time = table.Column<long>(type: "bigint", nullable: true),
                    time_interval = table.Column<long>(type: "bigint", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    remote_ip_address = table.Column<string>(type: "text", nullable: true),
                    remote_port = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_records", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "album",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    seq = table.Column<int>(type: "integer", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appautor = table.Column<string>(type: "text", nullable: false),
                    appdetail = table.Column<string>(type: "text", nullable: false),
                    appname = table.Column<string>(type: "text", nullable: false),
                    apppackage = table.Column<string>(type: "text", nullable: false),
                    appdownurl = table.Column<string>(type: "text", nullable: false),
                    applogourl = table.Column<string>(type: "text", nullable: false),
                    appversion = table.Column<string>(type: "text", nullable: false),
                    imglist = table.Column<string[]>(type: "text[]", nullable: false),
                    appsize = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_info", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "attach_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    upload_user_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    storage_type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attach_info", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blog_comment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    content = table.Column<string>(type: "text", nullable: false),
                    to_comment_id = table.Column<long>(type: "bigint", nullable: true),
                    to_user_id = table.Column<long>(type: "bigint", nullable: true),
                    like_num = table.Column<int>(type: "integer", nullable: false),
                    like_user_ids = table.Column<long?[]>(type: "bigint[]", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    allow = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_comment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blog_comment_public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_i_like_it = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_comment_public", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blog_info_custom",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    username = table.Column<string>(type: "text", nullable: false),
                    user_logo = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    sub_title = table.Column<string>(type: "text", nullable: false),
                    logo = table.Column<string>(type: "text", nullable: false),
                    open_comment = table.Column<string>(type: "text", nullable: false),
                    comment_type = table.Column<string>(type: "text", nullable: false),
                    theme_id = table.Column<string>(type: "text", nullable: false),
                    sub_domain = table.Column<string>(type: "text", nullable: false),
                    domain = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_info_custom", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blog_like",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_like", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blog_single",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    url_title = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_single", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blog_stat",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    node_id = table.Column<long>(type: "bigint", nullable: true),
                    read_num = table.Column<int>(type: "integer", nullable: false),
                    like_num = table.Column<int>(type: "integer", nullable: false),
                    comment_num = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_stat", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cate",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parent_cate_id = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    url_title = table.Column<string>(type: "text", nullable: false),
                    CateId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cate", x => x.id);
                    table.ForeignKey(
                        name: "FK_cate_cate_CateId",
                        column: x => x.CateId,
                        principalTable: "cate",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "commodity_order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mchid = table.Column<string>(type: "text", nullable: false),
                    total_fee = table.Column<int>(type: "integer", nullable: false),
                    out_trade_no = table.Column<string>(type: "text", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    attch = table.Column<string>(type: "text", nullable: false),
                    notify_url = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true),
                    payjs_order_id = table.Column<string>(type: "text", nullable: false),
                    transaction_id = table.Column<string>(type: "text", nullable: true),
                    openid = table.Column<string>(type: "text", nullable: true),
                    notify = table.Column<bool>(type: "boolean", nullable: false),
                    pay_status = table.Column<bool>(type: "boolean", nullable: false),
                    re_fund = table.Column<bool>(type: "boolean", nullable: false),
                    native_request_message = table.Column<string>(type: "text", nullable: true),
                    native_response_message = table.Column<string>(type: "text", nullable: true),
                    notify_response_message = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commodity_order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_sign",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    data_id = table.Column<long>(type: "bigint", nullable: true),
                    tag = table.Column<string>(type: "text", nullable: false),
                    data_sign_json = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_sign", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "email_log",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    msg = table.Column<string>(type: "text", nullable: false),
                    ok = table.Column<bool>(type: "boolean", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "file_repository",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    license = table.Column<string>(type: "text", nullable: true),
                    owner_type = table.Column<int>(type: "integer", nullable: false),
                    owner_id = table.Column<long>(type: "bigint", nullable: true),
                    visible = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_repository", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "friend_links",
                columns: table => new
                {
                    friend_links_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    theme_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friend_links", x => x.friend_links_id);
                });

            migrationBuilder.CreateTable(
                name: "group_team",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    user_count = table.Column<int>(type: "integer", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_team", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "group_team_user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_team_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logging_login",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    login_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    login_method = table.Column<string>(type: "text", nullable: true),
                    is_login_success = table.Column<bool>(type: "boolean", nullable: false),
                    error_meesage = table.Column<string>(type: "text", nullable: true),
                    ip = table.Column<string>(type: "text", nullable: true),
                    browser_request_header = table.Column<string>(type: "text", nullable: true),
                    hmac = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logging_login", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "note",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    notes_repository_id = table.Column<long>(type: "bigint", nullable: true),
                    created_user_id = table.Column<long>(type: "bigint", nullable: true),
                    notebook_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    title_vector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true),
                    desc = table.Column<string>(type: "text", nullable: true),
                    src = table.Column<string>(type: "text", nullable: true),
                    img_src = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<string[]>(type: "text[]", nullable: true),
                    is_trash = table.Column<bool>(type: "boolean", nullable: false),
                    is_blog = table.Column<bool>(type: "boolean", nullable: false),
                    url_title = table.Column<string>(type: "text", nullable: false),
                    is_recommend = table.Column<bool>(type: "boolean", nullable: false),
                    is_top = table.Column<bool>(type: "boolean", nullable: false),
                    has_self_defined = table.Column<bool>(type: "boolean", nullable: false),
                    read_num = table.Column<int>(type: "integer", nullable: false),
                    like_num = table.Column<int>(type: "integer", nullable: false),
                    comment_num = table.Column<int>(type: "integer", nullable: false),
                    is_markdown = table.Column<bool>(type: "boolean", nullable: false),
                    attach_num = table.Column<int>(type: "integer", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    recommend_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    public_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_user_id = table.Column<long>(type: "bigint", nullable: true),
                    usn = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    is_public_share = table.Column<bool>(type: "boolean", nullable: false),
                    content_id = table.Column<long>(type: "bigint", nullable: true),
                    access_password = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "note_content",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    is_blog = table.Column<bool>(type: "boolean", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    content_vector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true),
                    @abstract = table.Column<string>(name: "abstract", type: "text", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_user_id = table.Column<long>(type: "bigint", nullable: true),
                    is_history = table.Column<bool>(type: "boolean", nullable: false),
                    is_encryption = table.Column<bool>(type: "boolean", nullable: false),
                    hmac = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_content", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "note_file",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    notes_repository_id = table.Column<long>(type: "bigint", nullable: true),
                    album_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true),
                    path = table.Column<string>(type: "text", nullable: false),
                    is_default_album = table.Column<bool>(type: "boolean", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    from_file_id = table.Column<long>(type: "bigint", nullable: true),
                    number_of_file_references = table.Column<int>(type: "integer", nullable: false),
                    sha1 = table.Column<string>(type: "text", nullable: true),
                    md5 = table.Column<string>(type: "text", nullable: true),
                    storage_type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_file", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "note_image",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    image_id = table.Column<long>(type: "bigint", nullable: true),
                    use_count = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_image", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "note_tag",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    tag = table.Column<string>(type: "text", nullable: false),
                    usn = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    is_blog = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_tag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "note_tag_map",
                columns: table => new
                {
                    note_tag_map_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    note_tag_id = table.Column<long>(type: "bigint", nullable: true),
                    is_blog = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_tag_map", x => x.note_tag_map_id);
                });

            migrationBuilder.CreateTable(
                name: "notebook",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    notes_repository_id = table.Column<long>(type: "bigint", nullable: true),
                    parent_notebook_Id = table.Column<long>(type: "bigint", nullable: true),
                    seq = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    url_title = table.Column<string>(type: "text", nullable: false),
                    number_notes = table.Column<int>(type: "integer", nullable: false),
                    is_trash = table.Column<bool>(type: "boolean", nullable: false),
                    is_blog = table.Column<bool>(type: "boolean", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_wx = table.Column<bool>(type: "boolean", nullable: false),
                    usn = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notebook", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notes_repository",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    star_counter = table.Column<int>(type: "integer", nullable: false),
                    fork_counter = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    license = table.Column<string>(type: "text", nullable: true),
                    owner_type = table.Column<int>(type: "integer", nullable: false),
                    owner_id = table.Column<long>(type: "bigint", nullable: true),
                    visible = table.Column<bool>(type: "boolean", nullable: false),
                    usn = table.Column<int>(type: "integer", nullable: false),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false),
                    is_blog = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notes_repository", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organization",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    owner_id = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organization_member",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organization_id = table.Column<long>(type: "bigint", nullable: true),
                    role_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization_member", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organization_member_role",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organization_id = table.Column<long>(type: "bigint", nullable: true),
                    role_name = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization_member_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organization_member_role_mapping",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<long>(type: "bigint", nullable: true),
                    authority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization_member_role_mapping", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organization_team",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    organization_id = table.Column<long>(type: "bigint", nullable: true),
                    role_id = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization_team", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organization_team_member",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization_team_member", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    url_title = table.Column<string>(type: "text", nullable: false),
                    img_src = table.Column<string>(type: "text", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    public_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    desc = table.Column<string>(type: "text", nullable: false),
                    @abstract = table.Column<string>(name: "abstract", type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    tags = table.Column<string[]>(type: "text[]", nullable: false),
                    comment_num = table.Column<int>(type: "integer", nullable: false),
                    read_num = table.Column<int>(type: "integer", nullable: false),
                    like_num = table.Column<int>(type: "integer", nullable: false),
                    is_markdown = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "random_image",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_name = table.Column<string>(type: "text", nullable: false),
                    type_name_md5 = table.Column<string>(type: "text", nullable: false),
                    type_name_sha1 = table.Column<string>(type: "text", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    file_name_md5 = table.Column<string>(type: "text", nullable: false),
                    file_name_sha1 = table.Column<string>(type: "text", nullable: false),
                    file_sha1 = table.Column<string>(type: "text", nullable: false),
                    sex = table.Column<bool>(type: "boolean", nullable: false),
                    block = table.Column<bool>(type: "boolean", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false),
                    is_302 = table.Column<bool>(type: "boolean", nullable: false),
                    external_link = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_random_image", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "real_name_information",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    id_card_no = table.Column<string>(type: "text", nullable: true),
                    is_encryption = table.Column<bool>(type: "boolean", nullable: false),
                    hmac = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_real_name_information", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "report_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    reason = table.Column<string>(type: "text", nullable: false),
                    comment_id = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_info", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "repository_member",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<long>(type: "bigint", nullable: true),
                    respository_id = table.Column<long>(type: "bigint", nullable: true),
                    accessor_id = table.Column<long>(type: "bigint", nullable: true),
                    accessor_type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repository_member", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "repository_member_Role",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "text", nullable: true),
                    repository_id = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repository_member_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "repository_member_role_mapping",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<long>(type: "bigint", nullable: true),
                    authority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repository_member_role_mapping", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resolution_location",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    strategy_id = table.Column<long>(type: "bigint", nullable: true),
                    url = table.Column<string>(type: "text", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    speed = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resolution_location", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resolution_strategy",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    strategy_key = table.Column<string>(type: "text", nullable: false),
                    strategy_name = table.Column<string>(type: "text", nullable: false),
                    check_time = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resolution_strategy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<long>(type: "bigint", nullable: true),
                    login_times = table.Column<int>(type: "integer", nullable: false),
                    captcha = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "spam_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    spam_input = table.Column<string>(type: "text", nullable: false),
                    prediction = table.Column<bool>(type: "boolean", nullable: false),
                    score = table.Column<float>(type: "real", nullable: false),
                    manual_check = table.Column<bool>(type: "boolean", nullable: false),
                    manual_result = table.Column<bool>(type: "boolean", nullable: false),
                    creat_data = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spam_info", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suggestion",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    addr = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suggestion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tags = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "tag_count",
                columns: table => new
                {
                    tag_count_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    tag = table.Column<string>(type: "text", nullable: false),
                    is_blog = table.Column<bool>(type: "boolean", nullable: false),
                    tag_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_count", x => x.tag_count_id);
                });

            migrationBuilder.CreateTable(
                name: "theme",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<string>(type: "text", nullable: false),
                    author = table.Column<string>(type: "text", nullable: false),
                    author_url = table.Column<string>(type: "text", nullable: false),
                    theme_path = table.Column<string>(type: "text", nullable: false),
                    info = table.Column<string[]>(type: "text[]", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    style = table.Column<string>(type: "text", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theme", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "token",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    token_str = table.Column<string>(type: "text", nullable: false),
                    token_tag = table.Column<string>(type: "text", nullable: true),
                    token_type = table.Column<int>(type: "integer", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_token", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_account",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    account_type = table.Column<string>(type: "text", nullable: false),
                    account_start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    account_end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    max_image_num = table.Column<int>(type: "integer", nullable: false),
                    max_image_size = table.Column<int>(type: "integer", nullable: false),
                    max_attach_Num = table.Column<int>(type: "integer", nullable: false),
                    max_attach_size = table.Column<int>(type: "integer", nullable: false),
                    max_per_attach_size = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_blog",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    logo = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    sub_title = table.Column<string>(type: "text", nullable: true),
                    about_me = table.Column<string>(type: "text", nullable: true),
                    can_comment = table.Column<bool>(type: "boolean", nullable: false),
                    comment_type = table.Column<string>(type: "text", nullable: true),
                    disqus_id = table.Column<string>(type: "text", nullable: true),
                    style = table.Column<string>(type: "text", nullable: true),
                    css = table.Column<string>(type: "text", nullable: true),
                    theme_id = table.Column<long>(type: "bigint", nullable: true),
                    theme_path = table.Column<string>(type: "text", nullable: true),
                    cate_ids = table.Column<string[]>(type: "text[]", nullable: true),
                    singles = table.Column<string[]>(type: "text[]", nullable: true),
                    per_page_size = table.Column<int>(type: "integer", nullable: false),
                    sort_field = table.Column<string>(type: "text", nullable: true),
                    is_asc = table.Column<bool>(type: "boolean", nullable: false),
                    sub_domain = table.Column<string>(type: "text", nullable: true),
                    domain = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_blog", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_blog_base",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    logo = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    sub_title = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_blog_base", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_blog_comment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    can_comment = table.Column<bool>(type: "boolean", nullable: false),
                    comment_type = table.Column<string>(type: "text", nullable: false),
                    disqus_id = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_blog_comment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_blog_style",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    style = table.Column<string>(type: "text", nullable: false),
                    css = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_blog_style", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: true),
                    verified = table.Column<bool>(type: "boolean", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    username_raw = table.Column<string>(type: "text", nullable: true),
                    pwd = table.Column<string>(type: "text", nullable: true),
                    password_hash_algorithm = table.Column<string>(type: "text", nullable: false),
                    password_hash_iterations = table.Column<int>(type: "integer", nullable: false),
                    password_degree_of_parallelism = table.Column<int>(type: "integer", nullable: false),
                    password_memory_size = table.Column<int>(type: "integer", nullable: false),
                    salt = table.Column<string>(type: "text", nullable: true),
                    google_authenticator_secret_key = table.Column<string>(type: "text", nullable: true),
                    user_role = table.Column<string>(type: "text", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    logo = table.Column<string>(type: "text", nullable: true),
                    theme = table.Column<string>(type: "text", nullable: true),
                    notebook_width = table.Column<int>(type: "integer", nullable: false),
                    note_list_width = table.Column<int>(type: "integer", nullable: false),
                    md_editor_width = table.Column<int>(type: "integer", nullable: false),
                    left_is_min = table.Column<bool>(type: "boolean", nullable: false),
                    third_user_id = table.Column<string>(type: "text", nullable: true),
                    third_username = table.Column<string>(type: "text", nullable: true),
                    third_type = table.Column<int>(type: "integer", nullable: false),
                    image_num = table.Column<int>(type: "integer", nullable: false),
                    image_size = table.Column<int>(type: "integer", nullable: false),
                    attach_num = table.Column<int>(type: "integer", nullable: false),
                    attach_size = table.Column<int>(type: "integer", nullable: false),
                    from_user_id = table.Column<long>(type: "bigint", nullable: true),
                    account_type = table.Column<int>(type: "integer", nullable: false),
                    account_start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    account_end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    max_image_num = table.Column<int>(type: "integer", nullable: false),
                    max_image_size = table.Column<int>(type: "integer", nullable: false),
                    max_attach_num = table.Column<int>(type: "integer", nullable: false),
                    max_attach_size = table.Column<int>(type: "integer", nullable: false),
                    max_per_attach_size = table.Column<int>(type: "integer", nullable: false),
                    usn = table.Column<int>(type: "integer", nullable: false),
                    full_sync_before = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    blog_url = table.Column<string>(type: "text", nullable: true),
                    post_url = table.Column<string>(type: "text", nullable: true),
                    profile = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: false),
                    geographic_province = table.Column<string>(type: "text", nullable: false),
                    geographic_city = table.Column<string>(type: "text", nullable: false),
                    signature = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    group = table.Column<string>(type: "text", nullable: false),
                    tags = table.Column<string>(type: "text", nullable: false),
                    markdown_option = table.Column<string>(type: "text", nullable: true),
                    rich_text_option = table.Column<string>(type: "text", nullable: true),
                    password_expired = table.Column<int>(type: "integer", nullable: true),
                    change_password_reminder = table.Column<bool>(type: "boolean", nullable: true),
                    login_security_policy_level = table.Column<int>(type: "integer", nullable: false),
                    hmac = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_info", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "authorization",
                columns: table => new
                {
                    authorization_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    authorization_type = table.Column<string>(type: "text", nullable: false),
                    authorization_value = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authorization", x => x.authorization_id);
                    table.ForeignKey(
                        name: "FK_authorization_user_info_UserId",
                        column: x => x.UserId,
                        principalTable: "user_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "fido2_item",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    fido2_name = table.Column<string>(type: "text", nullable: true),
                    fido2_credential_id = table.Column<byte[]>(type: "bytea", nullable: true),
                    fido2_public_key = table.Column<byte[]>(type: "bytea", nullable: true),
                    fido2_user_handle = table.Column<byte[]>(type: "bytea", nullable: true),
                    fido2_signature_counter = table.Column<long>(type: "bigint", nullable: false),
                    fido2_cred_type = table.Column<string>(type: "text", nullable: true),
                    fido2_reg_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fido2_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fido2_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_fido2_item_user_info_user_id",
                        column: x => x.user_id,
                        principalTable: "user_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_records_ip_x_real_ip_x_forwarded_for_access_time_url",
                table: "access_records",
                columns: new[] { "ip", "x_real_ip", "x_forwarded_for", "access_time", "url" });

            migrationBuilder.CreateIndex(
                name: "IX_authorization_UserId",
                table: "authorization",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_cate_CateId",
                table: "cate",
                column: "CateId");

            migrationBuilder.CreateIndex(
                name: "IX_data_sign_id_data_id",
                table: "data_sign",
                columns: new[] { "id", "data_id" });

            migrationBuilder.CreateIndex(
                name: "IX_fido2_item_id",
                table: "fido2_item",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fido2_item_user_id",
                table: "fido2_item",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_logging_login_id_user_id_login_datetime_login_method_is_log~",
                table: "logging_login",
                columns: new[] { "id", "user_id", "login_datetime", "login_method", "is_login_success", "ip" });

            migrationBuilder.CreateIndex(
                name: "IX_note_user_id_is_blog_is_deleted",
                table: "note",
                columns: new[] { "user_id", "is_blog", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "IX_note_content_note_id_user_id_is_history",
                table: "note_content",
                columns: new[] { "note_id", "user_id", "is_history" });

            migrationBuilder.CreateIndex(
                name: "IX_note_file_id_user_id_sha1",
                table: "note_file",
                columns: new[] { "id", "user_id", "sha1" });

            migrationBuilder.CreateIndex(
                name: "IX_random_image_id_type_name_sex_is_delete_block_file_sha1",
                table: "random_image",
                columns: new[] { "id", "type_name", "sex", "is_delete", "block", "file_sha1" });

            migrationBuilder.CreateIndex(
                name: "IX_user_info_email_verified_username_username_raw_user_role_th~",
                table: "user_info",
                columns: new[] { "email", "verified", "username", "username_raw", "user_role", "third_user_id", "from_user_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_records");

            migrationBuilder.DropTable(
                name: "album");

            migrationBuilder.DropTable(
                name: "app_info");

            migrationBuilder.DropTable(
                name: "attach_info");

            migrationBuilder.DropTable(
                name: "authorization");

            migrationBuilder.DropTable(
                name: "blog_comment");

            migrationBuilder.DropTable(
                name: "blog_comment_public");

            migrationBuilder.DropTable(
                name: "blog_info_custom");

            migrationBuilder.DropTable(
                name: "blog_like");

            migrationBuilder.DropTable(
                name: "blog_single");

            migrationBuilder.DropTable(
                name: "blog_stat");

            migrationBuilder.DropTable(
                name: "cate");

            migrationBuilder.DropTable(
                name: "commodity_order");

            migrationBuilder.DropTable(
                name: "data_sign");

            migrationBuilder.DropTable(
                name: "email_log");

            migrationBuilder.DropTable(
                name: "fido2_item");

            migrationBuilder.DropTable(
                name: "file_repository");

            migrationBuilder.DropTable(
                name: "friend_links");

            migrationBuilder.DropTable(
                name: "group_team");

            migrationBuilder.DropTable(
                name: "group_team_user");

            migrationBuilder.DropTable(
                name: "logging_login");

            migrationBuilder.DropTable(
                name: "note");

            migrationBuilder.DropTable(
                name: "note_content");

            migrationBuilder.DropTable(
                name: "note_file");

            migrationBuilder.DropTable(
                name: "note_image");

            migrationBuilder.DropTable(
                name: "note_tag");

            migrationBuilder.DropTable(
                name: "note_tag_map");

            migrationBuilder.DropTable(
                name: "notebook");

            migrationBuilder.DropTable(
                name: "notes_repository");

            migrationBuilder.DropTable(
                name: "organization");

            migrationBuilder.DropTable(
                name: "organization_member");

            migrationBuilder.DropTable(
                name: "organization_member_role");

            migrationBuilder.DropTable(
                name: "organization_member_role_mapping");

            migrationBuilder.DropTable(
                name: "organization_team");

            migrationBuilder.DropTable(
                name: "organization_team_member");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "random_image");

            migrationBuilder.DropTable(
                name: "real_name_information");

            migrationBuilder.DropTable(
                name: "report_info");

            migrationBuilder.DropTable(
                name: "repository_member");

            migrationBuilder.DropTable(
                name: "repository_member_Role");

            migrationBuilder.DropTable(
                name: "repository_member_role_mapping");

            migrationBuilder.DropTable(
                name: "resolution_location");

            migrationBuilder.DropTable(
                name: "resolution_strategy");

            migrationBuilder.DropTable(
                name: "session");

            migrationBuilder.DropTable(
                name: "spam_info");

            migrationBuilder.DropTable(
                name: "suggestion");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "tag_count");

            migrationBuilder.DropTable(
                name: "theme");

            migrationBuilder.DropTable(
                name: "token");

            migrationBuilder.DropTable(
                name: "user_account");

            migrationBuilder.DropTable(
                name: "user_blog");

            migrationBuilder.DropTable(
                name: "user_blog_base");

            migrationBuilder.DropTable(
                name: "user_blog_comment");

            migrationBuilder.DropTable(
                name: "user_blog_style");

            migrationBuilder.DropTable(
                name: "user_info");
        }
    }
}
