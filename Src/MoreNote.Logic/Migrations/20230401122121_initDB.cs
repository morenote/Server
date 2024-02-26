using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

using NpgsqlTypes;

using System;
using System.Collections.Generic;

#nullable disable

namespace MoreNote.Logic.Migrations
{
	/// <inheritdoc />
	public partial class initDB : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "access_records",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ip = table.Column<string>(type: "text", nullable: true),
					xrealip = table.Column<string>(name: "x_real_ip", type: "text", nullable: true),
					xforwardedfor = table.Column<string>(name: "x_forwarded_for", type: "text", nullable: true),
					referrer = table.Column<string>(type: "text", nullable: true),
					requestheader = table.Column<string>(name: "request_header", type: "text", nullable: true),
					accesstime = table.Column<DateTime>(name: "access_time", type: "timestamp without time zone", nullable: false),
					unixtime = table.Column<long>(name: "unix_time", type: "bigint", nullable: true),
					timeinterval = table.Column<long>(name: "time_interval", type: "bigint", nullable: true),
					url = table.Column<string>(type: "text", nullable: true),
					remoteipaddress = table.Column<string>(name: "remote_ip_address", type: "text", nullable: true),
					remoteport = table.Column<string>(name: "remote_port", type: "text", nullable: true)
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
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					name = table.Column<string>(type: "text", nullable: false),
					type = table.Column<int>(type: "integer", nullable: false),
					seq = table.Column<int>(type: "integer", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_album", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "attach_info",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					noteid = table.Column<long>(name: "note_id", type: "bigint", nullable: true),
					uploaduserid = table.Column<long>(name: "upload_user_id", type: "bigint", nullable: true),
					name = table.Column<string>(type: "text", nullable: false),
					title = table.Column<string>(type: "text", nullable: false),
					size = table.Column<long>(type: "bigint", nullable: false),
					type = table.Column<string>(type: "text", nullable: false),
					path = table.Column<string>(type: "text", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					storagetype = table.Column<int>(name: "storage_type", type: "integer", nullable: false)
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
					noteid = table.Column<long>(name: "note_id", type: "bigint", nullable: true),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					content = table.Column<string>(type: "text", nullable: false),
					tocommentid = table.Column<long>(name: "to_comment_id", type: "bigint", nullable: true),
					touserid = table.Column<long>(name: "to_user_id", type: "bigint", nullable: true),
					likenum = table.Column<int>(name: "like_num", type: "integer", nullable: false),
					likeuserids = table.Column<long?[]>(name: "like_user_ids", type: "bigint[]", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					allow = table.Column<bool>(type: "boolean", nullable: false)
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
					isilikeit = table.Column<bool>(name: "is_i_like_it", type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_blog_comment_public", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "blog_hosting_bundle",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					host = table.Column<string>(type: "text", nullable: true),
					blogid = table.Column<long>(name: "blog_id", type: "bigint", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_blog_hosting_bundle", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "blog_info_custom",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					username = table.Column<string>(type: "text", nullable: false),
					userlogo = table.Column<string>(name: "user_logo", type: "text", nullable: false),
					title = table.Column<string>(type: "text", nullable: false),
					subtitle = table.Column<string>(name: "sub_title", type: "text", nullable: false),
					logo = table.Column<string>(type: "text", nullable: false),
					opencomment = table.Column<string>(name: "open_comment", type: "text", nullable: false),
					commenttype = table.Column<string>(name: "comment_type", type: "text", nullable: false),
					themeid = table.Column<string>(name: "theme_id", type: "text", nullable: false),
					subdomain = table.Column<string>(name: "sub_domain", type: "text", nullable: false),
					domain = table.Column<string>(type: "text", nullable: false)
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
					noteid = table.Column<long>(name: "note_id", type: "bigint", nullable: true),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false)
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
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					title = table.Column<string>(type: "text", nullable: false),
					urltitle = table.Column<string>(name: "url_title", type: "text", nullable: false),
					content = table.Column<string>(type: "text", nullable: false),
					updatedtime = table.Column<DateTime>(name: "updated_time", type: "timestamp without time zone", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false)
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
					nodeid = table.Column<long>(name: "node_id", type: "bigint", nullable: true),
					readnum = table.Column<int>(name: "read_num", type: "integer", nullable: false),
					likenum = table.Column<int>(name: "like_num", type: "integer", nullable: false),
					commentnum = table.Column<int>(name: "comment_num", type: "integer", nullable: false)
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
					parentcateid = table.Column<string>(name: "parent_cate_id", type: "text", nullable: false),
					title = table.Column<string>(type: "text", nullable: false),
					urltitle = table.Column<string>(name: "url_title", type: "text", nullable: false),
					BlogCateId = table.Column<long>(type: "bigint", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_cate", x => x.id);
					table.ForeignKey(
						name: "FK_cate_cate_BlogCateId",
						column: x => x.BlogCateId,
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
					totalfee = table.Column<int>(name: "total_fee", type: "integer", nullable: false),
					outtradeno = table.Column<string>(name: "out_trade_no", type: "text", nullable: false),
					body = table.Column<string>(type: "text", nullable: false),
					attch = table.Column<string>(type: "text", nullable: false),
					notifyurl = table.Column<string>(name: "notify_url", type: "text", nullable: false),
					type = table.Column<string>(type: "text", nullable: true),
					payjsorderid = table.Column<string>(name: "payjs_order_id", type: "text", nullable: false),
					transactionid = table.Column<string>(name: "transaction_id", type: "text", nullable: true),
					openid = table.Column<string>(type: "text", nullable: true),
					notify = table.Column<bool>(type: "boolean", nullable: false),
					paystatus = table.Column<bool>(name: "pay_status", type: "boolean", nullable: false),
					refund = table.Column<bool>(name: "re_fund", type: "boolean", nullable: false),
					nativerequestmessage = table.Column<string>(name: "native_request_message", type: "text", nullable: true),
					nativeresponsemessage = table.Column<string>(name: "native_response_message", type: "text", nullable: true),
					notifyresponsemessage = table.Column<string>(name: "notify_response_message", type: "text", nullable: true)
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
					dataid = table.Column<long>(name: "data_id", type: "bigint", nullable: true),
					tag = table.Column<string>(type: "text", nullable: false),
					datasignjson = table.Column<string>(name: "data_sign_json", type: "text", nullable: false)
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
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_email_log", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "friend_links",
				columns: table => new
				{
					friendlinksid = table.Column<long>(name: "friend_links_id", type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					themeid = table.Column<long>(name: "theme_id", type: "bigint", nullable: true),
					title = table.Column<string>(type: "text", nullable: true),
					url = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_friend_links", x => x.friendlinksid);
				});

			migrationBuilder.CreateTable(
				name: "group_team",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					title = table.Column<string>(type: "text", nullable: false),
					usercount = table.Column<int>(name: "user_count", type: "integer", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false)
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
					groupid = table.Column<long>(name: "group_id", type: "bigint", nullable: true),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false)
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
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					logindatetime = table.Column<DateTime>(name: "login_datetime", type: "timestamp without time zone", nullable: false),
					loginmethod = table.Column<string>(name: "login_method", type: "text", nullable: true),
					isloginsuccess = table.Column<bool>(name: "is_login_success", type: "boolean", nullable: false),
					errormeesage = table.Column<string>(name: "error_meesage", type: "text", nullable: true),
					ip = table.Column<string>(type: "text", nullable: true),
					browserrequestheader = table.Column<string>(name: "browser_request_header", type: "text", nullable: true),
					hmac = table.Column<string>(type: "text", nullable: true)
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
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					notesrepositoryid = table.Column<long>(name: "notes_repository_id", type: "bigint", nullable: true),
					createduserid = table.Column<long>(name: "created_user_id", type: "bigint", nullable: true),
					notebookid = table.Column<long>(name: "notebook_id", type: "bigint", nullable: true),
					title = table.Column<string>(type: "text", nullable: true),
					titlevector = table.Column<NpgsqlTsVector>(name: "title_vector", type: "tsvector", nullable: true),
					desc = table.Column<string>(type: "text", nullable: true),
					src = table.Column<string>(type: "text", nullable: true),
					imgsrc = table.Column<string>(name: "img_src", type: "text", nullable: true),
					tags = table.Column<string[]>(type: "text[]", nullable: true),
					istrash = table.Column<bool>(name: "is_trash", type: "boolean", nullable: false),
					isblog = table.Column<bool>(name: "is_blog", type: "boolean", nullable: false),
					urltitle = table.Column<string>(name: "url_title", type: "text", nullable: false),
					isrecommend = table.Column<bool>(name: "is_recommend", type: "boolean", nullable: false),
					istop = table.Column<bool>(name: "is_top", type: "boolean", nullable: false),
					hasselfdefined = table.Column<bool>(name: "has_self_defined", type: "boolean", nullable: false),
					readnum = table.Column<int>(name: "read_num", type: "integer", nullable: false),
					likenum = table.Column<int>(name: "like_num", type: "integer", nullable: false),
					commentnum = table.Column<int>(name: "comment_num", type: "integer", nullable: false),
					ismarkdown = table.Column<bool>(name: "is_markdown", type: "boolean", nullable: false),
					attachnum = table.Column<int>(name: "attach_num", type: "integer", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					updatedtime = table.Column<DateTime>(name: "updated_time", type: "timestamp without time zone", nullable: false),
					recommendtime = table.Column<DateTime>(name: "recommend_time", type: "timestamp without time zone", nullable: false),
					publictime = table.Column<DateTime>(name: "public_time", type: "timestamp without time zone", nullable: false),
					updateduserid = table.Column<long>(name: "updated_user_id", type: "bigint", nullable: true),
					usn = table.Column<int>(type: "integer", nullable: false),
					isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false),
					ispublicshare = table.Column<bool>(name: "is_public_share", type: "boolean", nullable: false),
					contentid = table.Column<long>(name: "content_id", type: "bigint", nullable: true),
					accesspassword = table.Column<string>(name: "access_password", type: "text", nullable: true)
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
					noteid = table.Column<long>(name: "note_id", type: "bigint", nullable: true),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					isblog = table.Column<bool>(name: "is_blog", type: "boolean", nullable: false),
					content = table.Column<string>(type: "text", nullable: true),
					contentvector = table.Column<NpgsqlTsVector>(name: "content_vector", type: "tsvector", nullable: true),
					@abstract = table.Column<string>(name: "abstract", type: "text", nullable: true),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					updatedtime = table.Column<DateTime>(name: "updated_time", type: "timestamp without time zone", nullable: false),
					updateduserid = table.Column<long>(name: "updated_user_id", type: "bigint", nullable: true),
					ishistory = table.Column<bool>(name: "is_history", type: "boolean", nullable: false),
					isencryption = table.Column<bool>(name: "is_encryption", type: "boolean", nullable: false),
					hmac = table.Column<string>(type: "text", nullable: true)
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
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					notesrepositoryid = table.Column<long>(name: "notes_repository_id", type: "bigint", nullable: true),
					albumid = table.Column<long>(name: "album_id", type: "bigint", nullable: true),
					name = table.Column<string>(type: "text", nullable: true),
					title = table.Column<string>(type: "text", nullable: true),
					size = table.Column<long>(type: "bigint", nullable: false),
					type = table.Column<string>(type: "text", nullable: true),
					path = table.Column<string>(type: "text", nullable: false),
					isdefaultalbum = table.Column<bool>(name: "is_default_album", type: "boolean", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					fromfileid = table.Column<long>(name: "from_file_id", type: "bigint", nullable: true),
					numberoffilereferences = table.Column<int>(name: "number_of_file_references", type: "integer", nullable: false),
					sha1 = table.Column<string>(type: "text", nullable: true),
					md5 = table.Column<string>(type: "text", nullable: true),
					storagetype = table.Column<int>(name: "storage_type", type: "integer", nullable: false)
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
					noteid = table.Column<long>(name: "note_id", type: "bigint", nullable: true),
					imageid = table.Column<long>(name: "image_id", type: "bigint", nullable: true),
					usecount = table.Column<int>(name: "use_count", type: "integer", nullable: false)
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
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					tag = table.Column<string>(type: "text", nullable: false),
					usn = table.Column<int>(type: "integer", nullable: false),
					count = table.Column<int>(type: "integer", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					updatedtime = table.Column<DateTime>(name: "updated_time", type: "timestamp without time zone", nullable: false),
					isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false),
					isblog = table.Column<bool>(name: "is_blog", type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_note_tag", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "note_tag_map",
				columns: table => new
				{
					notetagmapid = table.Column<long>(name: "note_tag_map_id", type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					noteid = table.Column<long>(name: "note_id", type: "bigint", nullable: true),
					notetagid = table.Column<long>(name: "note_tag_id", type: "bigint", nullable: true),
					isblog = table.Column<bool>(name: "is_blog", type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_note_tag_map", x => x.notetagmapid);
				});

			migrationBuilder.CreateTable(
				name: "notebook",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					notesrepositoryid = table.Column<long>(name: "notes_repository_id", type: "bigint", nullable: true),
					parentnotebookId = table.Column<long>(name: "parent_notebook_Id", type: "bigint", nullable: true),
					seq = table.Column<int>(type: "integer", nullable: false),
					title = table.Column<string>(type: "text", nullable: false),
					urltitle = table.Column<string>(name: "url_title", type: "text", nullable: false),
					numbernotes = table.Column<int>(name: "number_notes", type: "integer", nullable: false),
					istrash = table.Column<bool>(name: "is_trash", type: "boolean", nullable: false),
					isblog = table.Column<bool>(name: "is_blog", type: "boolean", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					updatedtime = table.Column<DateTime>(name: "updated_time", type: "timestamp without time zone", nullable: false),
					iswx = table.Column<bool>(name: "is_wx", type: "boolean", nullable: false),
					usn = table.Column<int>(type: "integer", nullable: false),
					isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_notebook", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "organization",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					name = table.Column<string>(type: "text", nullable: true),
					ownerid = table.Column<long>(name: "owner_id", type: "bigint", nullable: true)
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
					organizationid = table.Column<long>(name: "organization_id", type: "bigint", nullable: true),
					roleid = table.Column<long>(name: "role_id", type: "bigint", nullable: true),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true)
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
					organizationid = table.Column<long>(name: "organization_id", type: "bigint", nullable: true),
					rolename = table.Column<string>(name: "role_name", type: "text", nullable: true)
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
					roleid = table.Column<long>(name: "role_id", type: "bigint", nullable: true),
					authority = table.Column<int>(type: "integer", nullable: false)
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
					organizationid = table.Column<long>(name: "organization_id", type: "bigint", nullable: true),
					roleid = table.Column<long>(name: "role_id", type: "bigint", nullable: true),
					description = table.Column<string>(type: "text", nullable: true)
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
					teamid = table.Column<long>(name: "team_id", type: "bigint", nullable: true),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true)
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
					noteid = table.Column<long>(name: "note_id", type: "bigint", nullable: true),
					title = table.Column<string>(type: "text", nullable: false),
					urltitle = table.Column<string>(name: "url_title", type: "text", nullable: false),
					imgsrc = table.Column<string>(name: "img_src", type: "text", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					updatedtime = table.Column<DateTime>(name: "updated_time", type: "timestamp without time zone", nullable: false),
					publictime = table.Column<DateTime>(name: "public_time", type: "timestamp without time zone", nullable: false),
					desc = table.Column<string>(type: "text", nullable: false),
					@abstract = table.Column<string>(name: "abstract", type: "text", nullable: false),
					content = table.Column<string>(type: "text", nullable: false),
					tags = table.Column<string[]>(type: "text[]", nullable: false),
					commentnum = table.Column<int>(name: "comment_num", type: "integer", nullable: false),
					readnum = table.Column<int>(name: "read_num", type: "integer", nullable: false),
					likenum = table.Column<int>(name: "like_num", type: "integer", nullable: false),
					ismarkdown = table.Column<bool>(name: "is_markdown", type: "boolean", nullable: false)
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
					typename = table.Column<string>(name: "type_name", type: "text", nullable: false),
					typenamemd5 = table.Column<string>(name: "type_name_md5", type: "text", nullable: false),
					typenamesha1 = table.Column<string>(name: "type_name_sha1", type: "text", nullable: false),
					filename = table.Column<string>(name: "file_name", type: "text", nullable: false),
					filenamemd5 = table.Column<string>(name: "file_name_md5", type: "text", nullable: false),
					filenamesha1 = table.Column<string>(name: "file_name_sha1", type: "text", nullable: false),
					filesha1 = table.Column<string>(name: "file_sha1", type: "text", nullable: false),
					sex = table.Column<bool>(type: "boolean", nullable: false),
					block = table.Column<bool>(type: "boolean", nullable: false),
					isdelete = table.Column<bool>(name: "is_delete", type: "boolean", nullable: false),
					is302 = table.Column<bool>(name: "is_302", type: "boolean", nullable: false),
					externallink = table.Column<string>(name: "external_link", type: "text", nullable: true)
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
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					idcardno = table.Column<string>(name: "id_card_no", type: "text", nullable: true),
					isencryption = table.Column<bool>(name: "is_encryption", type: "boolean", nullable: false),
					hmac = table.Column<string>(type: "text", nullable: true)
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
					noteid = table.Column<long>(name: "note_id", type: "bigint", nullable: true),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					reason = table.Column<string>(type: "text", nullable: false),
					commentid = table.Column<int>(name: "comment_id", type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_report_info", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "repository",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					name = table.Column<string>(type: "text", nullable: true),
					avatar = table.Column<string>(type: "text", nullable: true),
					starcounter = table.Column<int>(name: "star_counter", type: "integer", nullable: false),
					forkcounter = table.Column<int>(name: "fork_counter", type: "integer", nullable: false),
					description = table.Column<string>(type: "text", nullable: true),
					license = table.Column<string>(type: "text", nullable: true),
					ownertype = table.Column<int>(name: "owner_type", type: "integer", nullable: false),
					repositorytype = table.Column<int>(name: "repository_type", type: "integer", nullable: false),
					ownerid = table.Column<long>(name: "owner_id", type: "bigint", nullable: true),
					visible = table.Column<bool>(type: "boolean", nullable: false),
					usn = table.Column<int>(type: "integer", nullable: false),
					createtime = table.Column<DateTime>(name: "create_time", type: "timestamp without time zone", nullable: false),
					isdelete = table.Column<bool>(name: "is_delete", type: "boolean", nullable: false),
					isblog = table.Column<bool>(name: "is_blog", type: "boolean", nullable: false),
					virtualfilebasepath = table.Column<string>(name: "virtual_file_base_path", type: "text", nullable: true),
					virtualfileaccessor = table.Column<string>(name: "virtual_file_accessor", type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_repository", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "repository_member",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					roleid = table.Column<long>(name: "role_id", type: "bigint", nullable: true),
					respositoryid = table.Column<long>(name: "respository_id", type: "bigint", nullable: true),
					accessorid = table.Column<long>(name: "accessor_id", type: "bigint", nullable: true),
					accessortype = table.Column<int>(name: "accessor_type", type: "integer", nullable: false)
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
					rolename = table.Column<string>(name: "role_name", type: "text", nullable: true),
					repositoryid = table.Column<long>(name: "repository_id", type: "bigint", nullable: true)
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
					roleid = table.Column<long>(name: "role_id", type: "bigint", nullable: true),
					authority = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_repository_member_role_mapping", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "session",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					sessionid = table.Column<long>(name: "session_id", type: "bigint", nullable: true),
					logintimes = table.Column<int>(name: "login_times", type: "integer", nullable: false),
					captcha = table.Column<string>(type: "text", nullable: false),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					updatedtime = table.Column<DateTime>(name: "updated_time", type: "timestamp without time zone", nullable: false)
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
					spaminput = table.Column<string>(name: "spam_input", type: "text", nullable: false),
					prediction = table.Column<bool>(type: "boolean", nullable: false),
					score = table.Column<float>(type: "real", nullable: false),
					manualcheck = table.Column<bool>(name: "manual_check", type: "boolean", nullable: false),
					manualresult = table.Column<bool>(name: "manual_result", type: "boolean", nullable: false),
					creatdata = table.Column<DateTime>(name: "creat_data", type: "timestamp without time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_spam_info", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "submit_block",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					version = table.Column<int>(type: "integer", nullable: false),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					treeid = table.Column<long>(name: "tree_id", type: "bigint", nullable: true),
					date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
					height = table.Column<int>(type: "integer", nullable: false),
					preblockid = table.Column<long>(name: "pre_block_id", type: "bigint", nullable: true),
					preblockhash = table.Column<string>(name: "pre_block_hash", type: "text", nullable: true),
					submithash = table.Column<string>(name: "submit_hash", type: "text", nullable: true),
					blockhash = table.Column<string>(name: "block_hash", type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_submit_block", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "submit_operation",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					submitblockid = table.Column<long>(name: "submit_block_id", type: "bigint", nullable: true),
					method = table.Column<int>(type: "integer", nullable: false),
					targettype = table.Column<int>(name: "target_type", type: "integer", nullable: false),
					target = table.Column<long>(type: "bigint", nullable: true),
					attributes = table.Column<int>(type: "integer", nullable: false),
					data = table.Column<string>(type: "text", nullable: true),
					dataindex = table.Column<long>(name: "data_index", type: "bigint", nullable: true),
					datahash = table.Column<string>(name: "data_hash", type: "text", nullable: true),
					operationhash = table.Column<string>(name: "operation_hash", type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_submit_operation", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "submit_tree",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					owner = table.Column<long>(type: "bigint", nullable: true),
					height = table.Column<int>(type: "integer", nullable: false),
					root = table.Column<long>(type: "bigint", nullable: true),
					top = table.Column<long>(type: "bigint", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_submit_tree", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "suggestion",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					addr = table.Column<string>(type: "text", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_suggestion", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "tag",
				columns: table => new
				{
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					tags = table.Column<List<string>>(type: "text[]", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_tag", x => x.userid);
				});

			migrationBuilder.CreateTable(
				name: "tag_count",
				columns: table => new
				{
					tagcountid = table.Column<long>(name: "tag_count_id", type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					tag = table.Column<string>(type: "text", nullable: false),
					isblog = table.Column<bool>(name: "is_blog", type: "boolean", nullable: false),
					tagcount = table.Column<int>(name: "tag_count", type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_tag_count", x => x.tagcountid);
				});

			migrationBuilder.CreateTable(
				name: "theme",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					name = table.Column<string>(type: "text", nullable: false),
					version = table.Column<string>(type: "text", nullable: false),
					author = table.Column<string>(type: "text", nullable: false),
					authorurl = table.Column<string>(name: "author_url", type: "text", nullable: false),
					themepath = table.Column<string>(name: "theme_path", type: "text", nullable: false),
					info = table.Column<string[]>(type: "text[]", nullable: false),
					isactive = table.Column<bool>(name: "is_active", type: "boolean", nullable: false),
					isdefault = table.Column<bool>(name: "is_default", type: "boolean", nullable: false),
					style = table.Column<string>(type: "text", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					updatedtime = table.Column<DateTime>(name: "updated_time", type: "timestamp without time zone", nullable: false)
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
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					email = table.Column<string>(type: "text", nullable: true),
					tokenstr = table.Column<string>(name: "token_str", type: "text", nullable: false),
					tokentag = table.Column<string>(name: "token_tag", type: "text", nullable: true),
					tokentype = table.Column<int>(name: "token_type", type: "integer", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false)
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
					accounttype = table.Column<string>(name: "account_type", type: "text", nullable: false),
					accountstarttime = table.Column<DateTime>(name: "account_start_time", type: "timestamp without time zone", nullable: false),
					accountendtime = table.Column<DateTime>(name: "account_end_time", type: "timestamp without time zone", nullable: false),
					maximagenum = table.Column<int>(name: "max_image_num", type: "integer", nullable: false),
					maximagesize = table.Column<int>(name: "max_image_size", type: "integer", nullable: false),
					maxattachNum = table.Column<int>(name: "max_attach_Num", type: "integer", nullable: false),
					maxattachsize = table.Column<int>(name: "max_attach_size", type: "integer", nullable: false),
					maxperattachsize = table.Column<int>(name: "max_per_attach_size", type: "integer", nullable: false)
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
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					logo = table.Column<string>(type: "text", nullable: true),
					title = table.Column<string>(type: "text", nullable: true),
					subtitle = table.Column<string>(name: "sub_title", type: "text", nullable: true),
					aboutme = table.Column<string>(name: "about_me", type: "text", nullable: true),
					cancomment = table.Column<bool>(name: "can_comment", type: "boolean", nullable: false),
					commenttype = table.Column<string>(name: "comment_type", type: "text", nullable: true),
					disqusid = table.Column<string>(name: "disqus_id", type: "text", nullable: true),
					style = table.Column<string>(type: "text", nullable: true),
					css = table.Column<string>(type: "text", nullable: true),
					themeid = table.Column<long>(name: "theme_id", type: "bigint", nullable: true),
					themepath = table.Column<string>(name: "theme_path", type: "text", nullable: true),
					cateids = table.Column<string[]>(name: "cate_ids", type: "text[]", nullable: true),
					singles = table.Column<string[]>(type: "text[]", nullable: true),
					perpagesize = table.Column<int>(name: "per_page_size", type: "integer", nullable: false),
					sortfield = table.Column<string>(name: "sort_field", type: "text", nullable: true),
					isasc = table.Column<bool>(name: "is_asc", type: "boolean", nullable: false),
					subdomain = table.Column<string>(name: "sub_domain", type: "text", nullable: true),
					domain = table.Column<string>(type: "text", nullable: true)
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
					subtitle = table.Column<string>(name: "sub_title", type: "text", nullable: false)
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
					cancomment = table.Column<bool>(name: "can_comment", type: "boolean", nullable: false),
					commenttype = table.Column<string>(name: "comment_type", type: "text", nullable: false),
					disqusid = table.Column<string>(name: "disqus_id", type: "text", nullable: true)
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
					css = table.Column<string>(type: "text", nullable: false)
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
					usernameraw = table.Column<string>(name: "username_raw", type: "text", nullable: true),
					pwd = table.Column<string>(type: "text", nullable: true),
					passwordhashalgorithm = table.Column<string>(name: "password_hash_algorithm", type: "text", nullable: false),
					passwordhashiterations = table.Column<int>(name: "password_hash_iterations", type: "integer", nullable: false),
					passworddegreeofparallelism = table.Column<int>(name: "password_degree_of_parallelism", type: "integer", nullable: false),
					passwordmemorysize = table.Column<int>(name: "password_memory_size", type: "integer", nullable: false),
					salt = table.Column<string>(type: "text", nullable: true),
					googleauthenticatorsecretkey = table.Column<string>(name: "google_authenticator_secret_key", type: "text", nullable: true),
					userrole = table.Column<string>(name: "user_role", type: "text", nullable: false),
					createdtime = table.Column<DateTime>(name: "created_time", type: "timestamp without time zone", nullable: false),
					logo = table.Column<string>(type: "text", nullable: true),
					theme = table.Column<string>(type: "text", nullable: true),
					notebookwidth = table.Column<int>(name: "notebook_width", type: "integer", nullable: false),
					notelistwidth = table.Column<int>(name: "note_list_width", type: "integer", nullable: false),
					mdeditorwidth = table.Column<int>(name: "md_editor_width", type: "integer", nullable: false),
					leftismin = table.Column<bool>(name: "left_is_min", type: "boolean", nullable: false),
					thirduserid = table.Column<string>(name: "third_user_id", type: "text", nullable: true),
					thirdusername = table.Column<string>(name: "third_username", type: "text", nullable: true),
					thirdtype = table.Column<int>(name: "third_type", type: "integer", nullable: false),
					imagenum = table.Column<int>(name: "image_num", type: "integer", nullable: false),
					imagesize = table.Column<int>(name: "image_size", type: "integer", nullable: false),
					attachnum = table.Column<int>(name: "attach_num", type: "integer", nullable: false),
					attachsize = table.Column<int>(name: "attach_size", type: "integer", nullable: false),
					fromuserid = table.Column<long>(name: "from_user_id", type: "bigint", nullable: true),
					accounttype = table.Column<int>(name: "account_type", type: "integer", nullable: false),
					accountstarttime = table.Column<DateTime>(name: "account_start_time", type: "timestamp without time zone", nullable: false),
					accountendtime = table.Column<DateTime>(name: "account_end_time", type: "timestamp without time zone", nullable: false),
					maximagenum = table.Column<int>(name: "max_image_num", type: "integer", nullable: false),
					maximagesize = table.Column<int>(name: "max_image_size", type: "integer", nullable: false),
					maxattachnum = table.Column<int>(name: "max_attach_num", type: "integer", nullable: false),
					maxattachsize = table.Column<int>(name: "max_attach_size", type: "integer", nullable: false),
					maxperattachsize = table.Column<int>(name: "max_per_attach_size", type: "integer", nullable: false),
					usn = table.Column<int>(type: "integer", nullable: false),
					fullsyncbefore = table.Column<DateTime>(name: "full_sync_before", type: "timestamp without time zone", nullable: false),
					blogurl = table.Column<string>(name: "blog_url", type: "text", nullable: true),
					posturl = table.Column<string>(name: "post_url", type: "text", nullable: true),
					profile = table.Column<string>(type: "text", nullable: true),
					country = table.Column<string>(type: "text", nullable: true),
					address = table.Column<string>(type: "text", nullable: true),
					phone = table.Column<string>(type: "text", nullable: true),
					Avatar = table.Column<string>(type: "text", nullable: true),
					geographicprovince = table.Column<string>(name: "geographic_province", type: "text", nullable: true),
					geographiccity = table.Column<string>(name: "geographic_city", type: "text", nullable: true),
					signature = table.Column<string>(type: "text", nullable: true),
					title = table.Column<string>(type: "text", nullable: true),
					group = table.Column<string>(type: "text", nullable: true),
					tags = table.Column<string>(type: "text", nullable: true),
					markdownoption = table.Column<string>(name: "markdown_option", type: "text", nullable: true),
					richtextoption = table.Column<string>(name: "rich_text_option", type: "text", nullable: true),
					passwordexpired = table.Column<int>(name: "password_expired", type: "integer", nullable: true),
					changepasswordreminder = table.Column<bool>(name: "change_password_reminder", type: "boolean", nullable: true),
					loginsecuritypolicylevel = table.Column<int>(name: "login_security_policy_level", type: "integer", nullable: false),
					hmac = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_user_info", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "virtual_file_info",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					name = table.Column<string>(type: "text", nullable: true),
					modifydate = table.Column<DateTime>(name: "modify_date", type: "timestamp without time zone", nullable: true),
					size = table.Column<long>(type: "bigint", nullable: true),
					parentid = table.Column<long>(name: "parent_id", type: "bigint", nullable: true),
					repositoryid = table.Column<long>(name: "repository_id", type: "bigint", nullable: true),
					isdelete = table.Column<bool>(name: "is_delete", type: "boolean", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_virtual_file_info", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "virtual_folder_info",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					name = table.Column<string>(type: "text", nullable: true),
					modifydate = table.Column<DateTime>(name: "modify_date", type: "timestamp without time zone", nullable: true),
					parentid = table.Column<long>(name: "parent_id", type: "bigint", nullable: true),
					repositoryid = table.Column<long>(name: "repository_id", type: "bigint", nullable: true),
					isdelete = table.Column<bool>(name: "is_delete", type: "boolean", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_virtual_folder_info", x => x.id);
				});

			migrationBuilder.CreateTable(
				name: "authorization",
				columns: table => new
				{
					authorizationid = table.Column<long>(name: "authorization_id", type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					authorizationtype = table.Column<string>(name: "authorization_type", type: "text", nullable: false),
					authorizationvalue = table.Column<string>(name: "authorization_value", type: "text", nullable: false),
					UserInfoId = table.Column<long>(type: "bigint", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_authorization", x => x.authorizationid);
					table.ForeignKey(
						name: "FK_authorization_user_info_UserInfoId",
						column: x => x.UserInfoId,
						principalTable: "user_info",
						principalColumn: "id");
				});

			migrationBuilder.CreateTable(
				name: "fido2_item",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					fido2name = table.Column<string>(name: "fido2_name", type: "text", nullable: true),
					fido2credentialid = table.Column<byte[]>(name: "fido2_credential_id", type: "bytea", nullable: true),
					fido2publickey = table.Column<byte[]>(name: "fido2_public_key", type: "bytea", nullable: true),
					fido2userhandle = table.Column<byte[]>(name: "fido2_user_handle", type: "bytea", nullable: true),
					fido2signaturecounter = table.Column<long>(name: "fido2_signature_counter", type: "bigint", nullable: false),
					fido2credtype = table.Column<string>(name: "fido2_cred_type", type: "text", nullable: true),
					fido2regdate = table.Column<DateTime>(name: "fido2_reg_date", type: "timestamp without time zone", nullable: true),
					fido2guid = table.Column<Guid>(name: "fido2_guid", type: "uuid", nullable: true),
					UserInfoId = table.Column<long>(type: "bigint", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_fido2_item", x => x.id);
					table.ForeignKey(
						name: "FK_fido2_item_user_info_UserInfoId",
						column: x => x.UserInfoId,
						principalTable: "user_info",
						principalColumn: "id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_access_records_ip_x_real_ip_x_forwarded_for_access_time_url",
				table: "access_records",
				columns: new[] { "ip", "x_real_ip", "x_forwarded_for", "access_time", "url" });

			migrationBuilder.CreateIndex(
				name: "IX_authorization_UserInfoId",
				table: "authorization",
				column: "UserInfoId");

			migrationBuilder.CreateIndex(
				name: "IX_cate_BlogCateId",
				table: "cate",
				column: "BlogCateId");

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
				name: "IX_fido2_item_UserInfoId",
				table: "fido2_item",
				column: "UserInfoId");

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

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "access_records");

			migrationBuilder.DropTable(
				name: "album");

			migrationBuilder.DropTable(
				name: "attach_info");

			migrationBuilder.DropTable(
				name: "authorization");

			migrationBuilder.DropTable(
				name: "blog_comment");

			migrationBuilder.DropTable(
				name: "blog_comment_public");

			migrationBuilder.DropTable(
				name: "blog_hosting_bundle");

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
				name: "repository");

			migrationBuilder.DropTable(
				name: "repository_member");

			migrationBuilder.DropTable(
				name: "repository_member_Role");

			migrationBuilder.DropTable(
				name: "repository_member_role_mapping");

			migrationBuilder.DropTable(
				name: "session");

			migrationBuilder.DropTable(
				name: "spam_info");

			migrationBuilder.DropTable(
				name: "submit_block");

			migrationBuilder.DropTable(
				name: "submit_operation");

			migrationBuilder.DropTable(
				name: "submit_tree");

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
				name: "virtual_file_info");

			migrationBuilder.DropTable(
				name: "virtual_folder_info");

			migrationBuilder.DropTable(
				name: "user_info");
		}
	}
}
