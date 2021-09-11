using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MoreNote.Logic.Migrations
{
    public partial class AddBlogCreatedTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_records",
                columns: table => new
                {
                    access_id = table.Column<long>(type: "bigint", nullable: false)
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
                    remote_port = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_records", x => x.access_id);
                });

            migrationBuilder.CreateTable(
                name: "album",
                columns: table => new
                {
                    album_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    seq = table.Column<int>(type: "integer", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album", x => x.album_id);
                });

            migrationBuilder.CreateTable(
                name: "app_info",
                columns: table => new
                {
                    appid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appautor = table.Column<string>(type: "text", nullable: true),
                    appdetail = table.Column<string>(type: "text", nullable: true),
                    appname = table.Column<string>(type: "text", nullable: true),
                    apppackage = table.Column<string>(type: "text", nullable: true),
                    appdownurl = table.Column<string>(type: "text", nullable: true),
                    applogourl = table.Column<string>(type: "text", nullable: true),
                    appversion = table.Column<string>(type: "text", nullable: true),
                    imglist = table.Column<string[]>(type: "text[]", nullable: true),
                    appsize = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_info", x => x.appid);
                });

            migrationBuilder.CreateTable(
                name: "attach_info",
                columns: table => new
                {
                    attach_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    upload_user_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true),
                    path = table.Column<string>(type: "text", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    storage_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attach_info", x => x.attach_id);
                });

            migrationBuilder.CreateTable(
                name: "blog_comment",
                columns: table => new
                {
                    comment_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    to_comment_id = table.Column<long>(type: "bigint", nullable: true),
                    to_user_id = table.Column<long>(type: "bigint", nullable: true),
                    like_num = table.Column<int>(type: "integer", nullable: false),
                    like_user_ids = table.Column<long?[]>(type: "bigint[]", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    allow = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_comment", x => x.comment_id);
                });

            migrationBuilder.CreateTable(
                name: "blog_comment_public",
                columns: table => new
                {
                    blog_comment_public_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_i_like_it = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_comment_public", x => x.blog_comment_public_id);
                });

            migrationBuilder.CreateTable(
                name: "blog_info_custom",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: true),
                    user_logo = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    sub_title = table.Column<string>(type: "text", nullable: true),
                    logo = table.Column<string>(type: "text", nullable: true),
                    open_comment = table.Column<string>(type: "text", nullable: true),
                    comment_type = table.Column<string>(type: "text", nullable: true),
                    theme_id = table.Column<string>(type: "text", nullable: true),
                    sub_domain = table.Column<string>(type: "text", nullable: true),
                    domain = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_info_custom", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "blog_like",
                columns: table => new
                {
                    like_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_like", x => x.like_id);
                });

            migrationBuilder.CreateTable(
                name: "blog_single",
                columns: table => new
                {
                    single_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    url_title = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_single", x => x.single_id);
                });

            migrationBuilder.CreateTable(
                name: "blog_stat",
                columns: table => new
                {
                    node_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    read_num = table.Column<int>(type: "integer", nullable: false),
                    like_num = table.Column<int>(type: "integer", nullable: false),
                    comment_num = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_stat", x => x.node_id);
                });

            migrationBuilder.CreateTable(
                name: "cate",
                columns: table => new
                {
                    cate_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parent_cate_id = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    url_title = table.Column<string>(type: "text", nullable: true),
                    CateId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cate", x => x.cate_id);
                    table.ForeignKey(
                        name: "FK_cate_cate_CateId1",
                        column: x => x.CateId1,
                        principalTable: "cate",
                        principalColumn: "cate_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "commodity_order",
                columns: table => new
                {
                    commodity_order_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mchid = table.Column<string>(type: "text", nullable: true),
                    total_fee = table.Column<int>(type: "integer", nullable: false),
                    out_trade_no = table.Column<string>(type: "text", nullable: true),
                    body = table.Column<string>(type: "text", nullable: true),
                    attch = table.Column<string>(type: "text", nullable: true),
                    notify_url = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    payjs_order_id = table.Column<string>(type: "text", nullable: true),
                    transaction_id = table.Column<string>(type: "text", nullable: true),
                    openid = table.Column<string>(type: "text", nullable: true),
                    notify = table.Column<bool>(type: "boolean", nullable: false),
                    pay_status = table.Column<bool>(type: "boolean", nullable: false),
                    re_fund = table.Column<bool>(type: "boolean", nullable: false),
                    native_request_message = table.Column<string>(type: "text", nullable: true),
                    native_response_message = table.Column<string>(type: "text", nullable: true),
                    notify_response_message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commodity_order", x => x.commodity_order_id);
                });

            migrationBuilder.CreateTable(
                name: "config",
                columns: table => new
                {
                    config_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    key = table.Column<string>(type: "text", nullable: true),
                    value_str = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_config", x => x.config_id);
                });

            migrationBuilder.CreateTable(
                name: "email_log",
                columns: table => new
                {
                    log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: true),
                    subject = table.Column<string>(type: "text", nullable: true),
                    body = table.Column<string>(type: "text", nullable: true),
                    msg = table.Column<string>(type: "text", nullable: true),
                    ok = table.Column<bool>(type: "boolean", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_log", x => x.log_id);
                });

            migrationBuilder.CreateTable(
                name: "friend_links",
                columns: table => new
                {
                    friend_links_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    theme_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    friend_links_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friend_links", x => x.friend_links_id);
                });

            migrationBuilder.CreateTable(
                name: "group_team",
                columns: table => new
                {
                    group_team_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    user_count = table.Column<int>(type: "integer", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_team", x => x.group_team_id);
                });

            migrationBuilder.CreateTable(
                name: "group_team_user",
                columns: table => new
                {
                    group_team_user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_team_user", x => x.group_team_user_id);
                });

            migrationBuilder.CreateTable(
                name: "note",
                columns: table => new
                {
                    note_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_user_id = table.Column<long>(type: "bigint", nullable: true),
                    notebook_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    desc = table.Column<string>(type: "text", nullable: true),
                    src = table.Column<string>(type: "text", nullable: true),
                    img_src = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<string[]>(type: "text[]", nullable: true),
                    is_trash = table.Column<bool>(type: "boolean", nullable: false),
                    is_blog = table.Column<bool>(type: "boolean", nullable: false),
                    url_title = table.Column<string>(type: "text", nullable: true),
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
                    content_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note", x => x.note_id);
                });

            migrationBuilder.CreateTable(
                name: "note_content",
                columns: table => new
                {
                    note_content_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    is_blog = table.Column<bool>(type: "boolean", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    @abstract = table.Column<string>(name: "abstract", type: "text", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_user_id = table.Column<long>(type: "bigint", nullable: true),
                    is_history = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_content", x => x.note_content_id);
                });

            migrationBuilder.CreateTable(
                name: "note_file",
                columns: table => new
                {
                    file_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    album_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true),
                    path = table.Column<string>(type: "text", nullable: true),
                    is_default_album = table.Column<bool>(type: "boolean", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    from_file_id = table.Column<long>(type: "bigint", nullable: true),
                    number_of_file_references = table.Column<int>(type: "integer", nullable: false),
                    sha1 = table.Column<string>(type: "text", nullable: true),
                    md5 = table.Column<string>(type: "text", nullable: true),
                    storage_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_file", x => x.file_id);
                });

            migrationBuilder.CreateTable(
                name: "note_image",
                columns: table => new
                {
                    note_image_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    image_id = table.Column<long>(type: "bigint", nullable: true),
                    use_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_image", x => x.note_image_id);
                });

            migrationBuilder.CreateTable(
                name: "note_tag",
                columns: table => new
                {
                    tag_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    tag = table.Column<string>(type: "text", nullable: true),
                    usn = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_tag", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "notebook",
                columns: table => new
                {
                    notebook_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    parent_notebook_Id = table.Column<long>(type: "bigint", nullable: true),
                    seq = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    url_title = table.Column<string>(type: "text", nullable: true),
                    number_notes = table.Column<int>(type: "integer", nullable: false),
                    is_trash = table.Column<bool>(type: "boolean", nullable: false),
                    is_blog = table.Column<bool>(type: "boolean", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_wx = table.Column<bool>(type: "boolean", nullable: false),
                    usn = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notebook", x => x.notebook_id);
                });

            migrationBuilder.CreateTable(
                name: "page",
                columns: table => new
                {
                    page_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cur_page = table.Column<int>(type: "integer", nullable: false),
                    total_page = table.Column<int>(type: "integer", nullable: false),
                    per_page_size = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_page", x => x.page_id);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    note_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    url_title = table.Column<string>(type: "text", nullable: true),
                    img_src = table.Column<string>(type: "text", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    public_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    desc = table.Column<string>(type: "text", nullable: true),
                    @abstract = table.Column<string>(name: "abstract", type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<string[]>(type: "text[]", nullable: true),
                    comment_num = table.Column<int>(type: "integer", nullable: false),
                    read_num = table.Column<int>(type: "integer", nullable: false),
                    like_num = table.Column<int>(type: "integer", nullable: false),
                    is_markdown = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.note_id);
                });

            migrationBuilder.CreateTable(
                name: "random_image",
                columns: table => new
                {
                    random_image_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_name = table.Column<string>(type: "text", nullable: true),
                    type_name_md5 = table.Column<string>(type: "text", nullable: true),
                    type_name_sha1 = table.Column<string>(type: "text", nullable: true),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    file_name_md5 = table.Column<string>(type: "text", nullable: true),
                    file_name_sha1 = table.Column<string>(type: "text", nullable: true),
                    file_sha1 = table.Column<string>(type: "text", nullable: true),
                    sex = table.Column<bool>(type: "boolean", nullable: false),
                    block = table.Column<bool>(type: "boolean", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false),
                    is_302 = table.Column<bool>(type: "boolean", nullable: false),
                    external_link = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_random_image", x => x.random_image_id);
                });

            migrationBuilder.CreateTable(
                name: "report_info",
                columns: table => new
                {
                    report_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    note_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    reason = table.Column<string>(type: "text", nullable: true),
                    comment_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_info", x => x.report_id);
                });

            migrationBuilder.CreateTable(
                name: "resolution_location",
                columns: table => new
                {
                    resolution_location_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    strategy_id = table.Column<long>(type: "bigint", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    score = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    speed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resolution_location", x => x.resolution_location_id);
                });

            migrationBuilder.CreateTable(
                name: "resolution_strategy",
                columns: table => new
                {
                    strategy_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    strategy_key = table.Column<string>(type: "text", nullable: true),
                    strategy_name = table.Column<string>(type: "text", nullable: true),
                    check_time = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resolution_strategy", x => x.strategy_id);
                });

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<long>(type: "bigint", nullable: true),
                    login_times = table.Column<int>(type: "integer", nullable: false),
                    captcha = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "spam_info",
                columns: table => new
                {
                    spam_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    spam_input = table.Column<string>(type: "text", nullable: true),
                    prediction = table.Column<bool>(type: "boolean", nullable: false),
                    score = table.Column<float>(type: "real", nullable: false),
                    manual_check = table.Column<bool>(type: "boolean", nullable: false),
                    manual_result = table.Column<bool>(type: "boolean", nullable: false),
                    creat_data = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spam_info", x => x.spam_id);
                });

            migrationBuilder.CreateTable(
                name: "suggestion",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    addr = table.Column<string>(type: "text", nullable: true)
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
                    tags = table.Column<List<string>>(type: "text[]", nullable: true)
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
                    tag = table.Column<string>(type: "text", nullable: true),
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
                    theme_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<string>(type: "text", nullable: true),
                    author = table.Column<string>(type: "text", nullable: true),
                    author_url = table.Column<string>(type: "text", nullable: true),
                    theme_path = table.Column<string>(type: "text", nullable: true),
                    info = table.Column<string[]>(type: "text[]", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    style = table.Column<string>(type: "text", nullable: true),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theme", x => x.theme_id);
                });

            migrationBuilder.CreateTable(
                name: "token",
                columns: table => new
                {
                    token_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    token_str = table.Column<string>(type: "text", nullable: true),
                    token_tag = table.Column<string>(type: "text", nullable: true),
                    token_type = table.Column<int>(type: "integer", nullable: false),
                    created_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_token", x => x.token_id);
                });

            migrationBuilder.CreateTable(
                name: "user_account",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    account_type = table.Column<string>(type: "text", nullable: true),
                    account_start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    account_end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    max_image_num = table.Column<int>(type: "integer", nullable: false),
                    max_image_size = table.Column<int>(type: "integer", nullable: false),
                    max_attach_Num = table.Column<int>(type: "integer", nullable: false),
                    max_attach_size = table.Column<int>(type: "integer", nullable: false),
                    max_per_attach_size = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_account", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "user_blog",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    domain = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_blog", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "user_blog_base",
                columns: table => new
                {
                    logo = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    sub_title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_blog_base", x => x.logo);
                });

            migrationBuilder.CreateTable(
                name: "user_blog_comment",
                columns: table => new
                {
                    user_blog_comment_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    can_comment = table.Column<bool>(type: "boolean", nullable: false),
                    comment_type = table.Column<string>(type: "text", nullable: true),
                    disqus_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_blog_comment", x => x.user_blog_comment_id);
                });

            migrationBuilder.CreateTable(
                name: "user_blog_style",
                columns: table => new
                {
                    user_blog_style_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    style = table.Column<string>(type: "text", nullable: true),
                    css = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_blog_style", x => x.user_blog_style_id);
                });

            migrationBuilder.CreateTable(
                name: "user_info",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: true),
                    verified = table.Column<bool>(type: "boolean", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    username_raw = table.Column<string>(type: "text", nullable: true),
                    pwd = table.Column<string>(type: "text", nullable: true),
                    hash_algorithm = table.Column<string>(type: "text", nullable: true),
                    salt = table.Column<string>(type: "text", nullable: true),
                    google_authenticator_secret_key = table.Column<string>(type: "text", nullable: true),
                    pwd_cost = table.Column<int>(type: "integer", nullable: false),
                    user_role = table.Column<string>(type: "text", nullable: true),
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
                    full_sync_before = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_info", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "authorization",
                columns: table => new
                {
                    authorization_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    authorization_type = table.Column<string>(type: "text", nullable: true),
                    authorization_value = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authorization", x => x.authorization_id);
                    table.ForeignKey(
                        name: "FK_authorization_user_info_UserId",
                        column: x => x.UserId,
                        principalTable: "user_info",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_authorization_UserId",
                table: "authorization",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_cate_CateId1",
                table: "cate",
                column: "CateId1");
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
                name: "config");

            migrationBuilder.DropTable(
                name: "email_log");

            migrationBuilder.DropTable(
                name: "friend_links");

            migrationBuilder.DropTable(
                name: "group_team");

            migrationBuilder.DropTable(
                name: "group_team_user");

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
                name: "notebook");

            migrationBuilder.DropTable(
                name: "page");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "random_image");

            migrationBuilder.DropTable(
                name: "report_info");

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
