# Host: localhost  (Version: 5.5.53)
# Date: 2019-05-16 15:50:38
# Generator: MySQL-Front 5.3  (Build 4.234)

/*!40101 SET NAMES utf8 */;

#
# Structure for table "article"
#

DROP TABLE IF EXISTS `article`;
CREATE TABLE `article` (
  `ArticleId` varchar(255) NOT NULL DEFAULT '',
  `Title` varchar(255) DEFAULT '无标题' COMMENT '标题',
  `CreatUser` varchar(255) NOT NULL DEFAULT '0' COMMENT '创建人',
  `CreatTime` datetime DEFAULT '2018-01-01 00:00:00' COMMENT '创建日期',
  `Author` varchar(255) DEFAULT '0' COMMENT '作者',
  `Score` int(11) DEFAULT '0' COMMENT '评分',
  `ArticleType` varchar(255) DEFAULT 'default' COMMENT '文章类别',
  `Status` varchar(255) DEFAULT '0' COMMENT '状态',
  `Sources` varchar(255) DEFAULT '0' COMMENT '信息来源',
  `Summary` varchar(255) DEFAULT '没有摘要' COMMENT '内容摘要',
  `Description` varchar(2000) DEFAULT '没有描述' COMMENT '描述',
  `SEOTitle` varchar(255) DEFAULT '' COMMENT 'SEO标题',
  `SEOKeyWord` varchar(255) DEFAULT '' COMMENT 'SEO关键词',
  `SEODescription` varchar(255) DEFAULT '0' COMMENT 'SEO描述',
  `AllowComments` bit(1) DEFAULT b'0' COMMENT '允许评论',
  `Topping` bit(1) DEFAULT b'0' COMMENT '置顶',
  `Recommend` bit(1) DEFAULT b'0' COMMENT '推荐',
  `Hot` bit(1) DEFAULT b'0' COMMENT '热门',
  `TurnMap` bit(1) DEFAULT b'0' COMMENT '轮番图',
  `RecType` varchar(255) DEFAULT '0' COMMENT '推荐类型',
  PRIMARY KEY (`ArticleId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

#
# Data for table "article"
#

/*!40000 ALTER TABLE `article` DISABLE KEYS */;
INSERT INTO `article` VALUES ('22daa83bde614e5bae7000c2d7f5ece7','近代史','','0001-01-01 00:00:00','小何',0,'option1','on','没有','这就是摘要','这是摘要这是摘要这是摘要这是摘要这是摘要这是摘要这是摘要这是摘要这是摘要这是摘要这是摘要','标题SEO','关键词SEO','这是SEO描述',b'1',b'1',b'1',b'1',b'1',''),('c8dead6392a','西游记','','2018-12-23 23:15:00','吴承恩',0,'option3','on','中国古典','第1集 　　混沌未分天地乱，茫茫渺渺无人见。自从盘古破鸿蒙，开辟从兹清浊辨。覆载群生仰至仁，发明万物皆成善。欲知造化会元功，须看《西游释厄传》。感盘古开辟，三皇治世，五帝定伦，世界之间，遂分为四大部洲：曰东胜神洲，曰西牛贺洲，曰南赡部洲，曰北俱芦洲。这部书单表东胜神洲。海外有一国土，名曰傲来国。国近大海，海中有一座山，唤为花果山……承蒙天地造化，天气下降，地气上升，天地阴阳交合，在花果山正当顶上孕育出一块石卵……又经历了几百万个寒暑，石卵每受天真地秀，日精月华，感知既久，遂有灵通之意……忽一日，好个电闪雷','唐玄奘经菩萨指点奉唐朝天子之命前往西天取经。途中路经五行山收得齐天大圣为徒取名孙悟空，继而在高老庄又遇到了因调戏嫦娥被逐出天界的猪八戒。师徒三人来到流沙河，收服水妖取法名悟净，至此唐僧带着三个徒弟历经千难万险，八十一难，最终来到灵山，玄奘在凌云渡放下了肉身，终于取回了真经，回到大唐长安，把大乘佛法广宣流布……而一路勇敢无畏的孙悟空也成为斗战胜佛，八戒、沙僧、白龙马分别成为净坛使者、金身罗汉、八部天龙马……功德圆满。 [3] ','西游记','西游记','中国古典小说',b'1',b'1',b'1',b'1',b'1',''),('de08bf7dfd1740579a55e071a403d608','动漫','','0001-01-01 00:00:00','王咸强',0,'option1','','','','','','','',b'1',b'1',b'1',b'1',b'1',''),('wCKDOBJI3TP','唐诗三百首','','2018-01-01 11:23:00','李白',0,'option2','','唐诗精选文集','内容摘要','内容描述','流浪地球','key','描述',b'1',b'1',b'1',b'1',b'1','');
/*!40000 ALTER TABLE `article` ENABLE KEYS */;

#
# Structure for table "author"
#

DROP TABLE IF EXISTS `author`;
CREATE TABLE `author` (
  `Id` varchar(255) NOT NULL DEFAULT '',
  `Name` varchar(255) NOT NULL DEFAULT '匿名',
  `Description` varchar(255) NOT NULL DEFAULT '没有详情描述',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='作者索引';

#
# Data for table "author"
#

/*!40000 ALTER TABLE `author` DISABLE KEYS */;
INSERT INTO `author` VALUES ('19de819d6b9a42d4bc712d6890e9d551','不知名作何','这是李白'),('4eba6fd420634e91b807cc94437b1d89','李白','李白（701年－762年） ，字太白，号青莲居士，又号“谪仙人”，是唐代伟大的浪漫主义诗人，被后人誉为“诗仙”，与杜甫并称为“李杜”，为了与另两位诗人李商隐与杜牧即“小李杜”区别，杜甫与李白又合称“大李杜”。据《新唐书》记载，李白为兴圣皇帝（凉武昭王李暠）九世孙，与李唐诸王同宗。其人爽朗大方，爱饮酒作诗，喜交友。'),('cc2b49ea9acf461ca54c322590d6314b','吴承恩','吴承恩（1506年—约1583年），字汝忠，号射阳山人。汉族，淮安府山阳县人（现淮安市淮安区人）。祖籍安徽 ，以祖先聚居枞阳高甸，故称高甸吴氏。\n现存明刊百回本《西游记》均无作者署名，提出《西游记》作者是吴承恩的首先是清代学者吴玉搢，吴玉搢在《山阳志遗》中介绍吴承恩：“嘉靖中,吴贡生承恩,字汝忠,号射阳山人,吾淮才士也”，“及阅《淮贤文目》，载《西游记》为先生著” [1]  。吴承恩自幼敏慧，博览群书，尤喜爱神话故事。在科举中屡遭挫折，嘉靖中补贡生。嘉靖四十五年(1566年)任浙江长兴县丞。殊途由于宦途困'),('fa47263fab4542629b1ba89d90c52c65','小何','作家，泛指能以文化创作为业，写作的人，也特指文学创作上有盛名成就的人。因此，一般能被称为“作家”者，其作品大都能够获得出版发行，历史悠久。相对于“作者”一词而言，“作家”一词的褒义明显较强，所以这词很多时候会被用作为一种客套敬称，或作为一种提高自己身价的标签，流于溢美，因此，被称为“作家”的网上写手、自由撰稿人为数不少。');
/*!40000 ALTER TABLE `author` ENABLE KEYS */;

#
# Structure for table "chapter"
#

DROP TABLE IF EXISTS `chapter`;
CREATE TABLE `chapter` (
  `ChapterId` varchar(255) NOT NULL DEFAULT '0',
  `ArticleId` varchar(255) DEFAULT NULL COMMENT '所属漫画',
  `Content` varchar(9999) DEFAULT NULL COMMENT '内容',
  `SerialNumber` int(11) DEFAULT NULL COMMENT '序号',
  `Title` varchar(255) DEFAULT NULL COMMENT '名字',
  `CreatTime` datetime DEFAULT NULL COMMENT '创建时间',
  `Summary` varchar(2500) DEFAULT '文章摘要' COMMENT '章节摘要',
  PRIMARY KEY (`ChapterId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

#
# Data for table "chapter"
#

/*!40000 ALTER TABLE `chapter` DISABLE KEYS */;
INSERT INTO `chapter` VALUES ('3b2656d04a1d45c0ad158fffef422e0f','wCKDOBJI3TP','<p><br></p><p style=\"text-align: center;\"><img src=\"https://timgsa.baidu.com/timg?image&amp;quality=80&amp;size=b9999_10000&amp;sec=1551893543384&amp;di=17a7e93a2a0495c89e352c7fc94a558f&amp;imgtype=0&amp;src=http%3A%2F%2Fpic3.40017.cn%2Fscenery%2Fdestination%2F2015%2F04%2F18%2F03%2Fvrn29Y.jpg\">&nbsp;&nbsp;<br></p><p><br></p><h1 style=\"text-align: center;\">常常感到怀念和遗憾</h1><p>　　高二这一年，我认识了H姑娘。</p><p>　　在没有遇到H姑娘以前，我确实没有想过后面会有这样的一个朋友。事实上，我并不是一个好相处的人，像是一只刺猬，只要人一靠近，就会被扎得生疼。我藏在自己的世界，觉得外面的世界充满了恶意，偶尔留意翼翼探出头看看外面，再迅速缩回去。这样黯淡无光的日子，在遇到H姑娘以后，发生了翻天覆地的变化。</p><p>　　当我反应过来的时候，我和H姑娘就成为了朋友，我只记得她的故事让人心疼，我在心中暗自决定，要和她做一辈子的好朋友。至于我们是怎样成为朋友的，我给忘了。人的记忆有限，而我愚笨，记得的东西更有限，反正一去二来，我和H姑娘就成了朋友。说实话，我真怕H姑娘明白了会来揍我。但是就应不会，也许H姑娘已经不在意这些事情了。</p><p>　　大概最好的朋友，从来都不需要费尽心思讨好。就算是最平淡的相处，也会感到莫名的小温馨。[由Www.DuanMeiWen.Com整理]</p><p>　　H姑娘坐在我身旁的那个组，上课的时候，我喜欢给H姑娘写小纸条，也不是什么重要的事情，无非是一两句小情诗，H姑娘也会回我。两个人就这样，你回我一张，我回你一张。明明就在一个教室，却对这样的事情乐此不疲。</p><p>　　也许是冥冥之中自有注定，下学期换教室调座位，我最后如愿以偿，和H姑娘成为了同桌。她坐在我的右手边，我一偏头就能够望见她，而她一脸认真的写作业，傍晚的夕阳光线，柔和而不刺眼，洒在她的身上，一瞬间我竟有些看呆，期望时光，就定格在这个有她的画面。坐在一齐的时光仓促而温柔，H姑娘会给我讲世态炎凉，教我为人处世的道理，我虽然比H姑娘大，但是很多方面却不如她做得好。</p><p>　　H姑娘待我极好极好，什么叫做极好极好呢？或许那时候的H姑娘，将她认为全世界最好的东西，都一一给了我。她对我的好，从来不在眉眼，只在心上。她予我最深情的关心，叫我此后再也不能忘。</p><p>　　H姑娘为我买各种各样的小零食，作为一名吃货的我，吃到了很多很多好吃的东西。生病的时候很贴心的给我买药和面包，之后我很害怕生病，却不是怕影响学习，而是怕再生病时，身边少了那个嘘寒问暖的人。圣诞节的时候，收到了H姑娘给我买的玫瑰花，独一无二的一朵，之后几番辗转，我依然舍不得丢掉。生活琐事苦恼的倾听，考试失败时的鼓励，被老师骂时的暖心安慰，往事种种，历历在目。H姑娘带给我太多太多，温暖到流泪的记忆。对于H姑娘的好，我从来都难以言说，任何言语在这一刻仿佛都变得苍白无力。</p><p>　　日子并不是波澜不惊，我和H姑娘也会因为生活中鸡毛蒜皮的小事而冷战，谁都不想先低头开口，这时候一张小纸条就能够解决问题。传递的是纸条，承载的却是心意。上一秒还相互不理睬，下一秒又彼此喜笑颜开。那时候，时光还很长，长到我以为我们能够就这样到永久。</p><p>　　这样的完美时光，停留在高考结束前三个月。</p><p>　　周末回校园的路上，听说初中最好的那个朋友，在去年退学了，而我一无所知，这个消息再加之家里一些事情，我的情绪很糟糕。H姑娘和我说话，我也无精打采，许是这样的态度恼了她，她就换了座位，不再理我。课间休息时，我拉上朋友，坐在石头上哭得很悲哀，H姑娘，不是我不理你，而是我真的很难过。第二天，H姑娘没有换来我身旁，当我再次写小纸条给她，得到的回复却是就这样吧，明明给我买了好丽友，回复我的却是不再是朋友。</p><p>　　坐车回家的路上，我还想着以后我们要是和好了，就把这段时间我的坎坷、紧张、惶恐，通通都说给她听，甚至略为夸张的想起一句歌词“相爱没有那么容易，每个人都有他的脾气”。我以为过不了多久我们就会和以前一样，再笑着回顾这段时间彼此的情绪。</p><p>　　这一次，我猜中了开头却没有猜中结局。</p><p>　　是的，我们再也没有和好。</p><p>　　三个人坐一排，我们还坐在一齐，中间隔了一个人。我和H姑娘不再说话，不再传纸条，各自做各自的事情。数不清的校统考，老师们匆匆忙忙阅卷，又匆匆忙忙发回试卷。晚自习，H姑娘换去了后面坐，有学生在发上一次的考卷，我整理试卷时发现我的英语试卷不在，也没放在心上。直到前面的男生在座位旁捡起我的试卷还给我，我才明白身旁的女生刚刚扔下去的试卷，原先是我的。对了，身旁的女生就是一向和我们坐的姑娘，我们三个人。在H姑娘不理我之后，她也和我渐渐疏离。看着这个女生，我决定要换座位，与H姑娘无关。</p><p>　　当天晚上，我迅速换了座位，从此，和H姑娘不再是同桌。我不明白她是不是以为我当初调开与她有关，其实并不是。我坐在新的座位，不敢再回头看她，怕我会流泪。</p><p>　　这个时候，距离高考，还有11天。</p><p>　　大概就像失了魂魄，从那以后盯着理综卷半天却一个字都写不出来，想问清楚到底是因为什么，却又怕让她生气，只能作罢。高考在即，我却毫不在意，那个待我极好的人阿，怎样突然就形同陌路。</p><p>　　最最难过的时候，我坐在床上，边哭边撕了我们一齐传的那些纸条，那天晚上，我在宿舍哭了很久很久。</p><p>　　时光呼啸而过，高考结束。拿成绩的那天，H姑娘换了一个很漂亮的新发型，人基本都走完的那个时候，我站在寥寥无几的教室，透过窗户，看H姑娘离开，她考了多少分呢？她要报哪个大学呢？我不明白，我也没有勇气去问，没有人告诉我答案。我就只能，静静看着她的背影，然后告诉自己，我们以后，可能没有机会，再见面了吧？这一切，是否就是随着高中的结束而划上了不完美的句号？</p><p>　　以前在深夜里哭的狼狈不堪的是我，之后因为那些回忆痛哭不止的也是我。我以前答应了H姑娘许多事情，还没来得及实现，就突然分别。</p><p>　　原先相遇如此容易忘记，而离别却深深印在脑海里。</p><p>　　结束了，离开了，彼此开始新的生活。以为或许有一天，我会将H姑娘遗忘在时光的彼岸。但是，每当遇到什么相似的场景，我依然会想起H姑娘，然后有种想流泪的感觉。我还记得第一次发消息给H姑娘，为什么发消息给H姑娘呢？因为，那天早上在校医院看病，遇到了一个学姐，说起了以前的事，然后讲到H姑娘，我说，H姑娘是一个极好极好的人，对我很好。</p><p>　　“你们此刻会联系吗？”学姐有些好奇地问。</p><p>　　我沉默了好久，才说，没有了。</p><p>　　“为什么呢？”学姐很不懂，为什么以前玩的那么好的人，后面却没有了联系呢？</p><p>　　“我，我……我也不明白。”</p><p>　　我确实不明白，我一向很想念H姑娘，但是我不敢发消息给她。我不明白就应用什么理由再去联系她。</p><p>　　但是那天，我不知从何而来的勇气，发了消息给H姑娘，就想问问H姑娘，她还好吗？最近过的怎样样？H姑娘睡了没有回我，第二天回的我，彼时我在公选课上，有一瞬间失神。我和她，有多久没有联系了？只是寥寥数语，却让我几度哽咽。</p><p>　　之后我去H姑娘的空间，翻看她以前给我写的那些说说，以前叫我某同学的那个人阿，不在我身边了。</p><p>　　之后我妈时不时也会提起H姑娘，问怎样她从来都不来家里玩，而我解释她很忙的，哪里有那么多时间来玩。</p><p>　　之后我和H姑娘的联系也渐渐多了起来，我们已经回不去，但我期望我们能够重新开始，我拼命想要弥补过去的遗憾，却又怕她不喜。</p><p>　　之后我也会梦见H姑娘，梦里的她依然如以前一般秀丽大方。大概是念念不忘，必有回响。不管梦里是在哪里，只要是和H姑娘一齐，就好。</p><p>　　再之后的故事怎样样了呢？我也不明白，反正我亲爱的姑娘阿，也有了英俊帅气的男朋友，晒出来的照片甜死人。反正我明白她过得很好啦，身边并不缺乏要好的朋友。反正我们中间隔着山水重重，分别后就再也没有见过面。</p><p>　　我常常感到怀念，也常常感到遗憾。</p><p>　　我不明白未来会是什么样貌的，我们又会怎样样，联系或者不联系，见面或者不见面。</p><p>　　但是至少，她给予过我温暖细腻的照顾与关怀，是我狼狈青春里再也不肯忘记，贪恋至今的完美。</p><p>　　作者：寻安</p><p><br></p>',0,'常常感到怀念和遗憾','2019-03-06 22:44:55','　高二这一年，我认识了H姑娘。\n\n　　在没有遇到H姑娘以前，我确实没有想过后面会有这样的一个朋友。事实上，我并不是一个好相处的人，像是一只刺猬，只要人一靠近，就会被扎得生疼。我藏在自己的世界，觉得外面的世界充满了恶意，偶尔留意翼翼探出头看看外面，再迅速缩回去。这样黯淡无光的日子，在遇到H姑娘以后，发生了翻天覆地的变化。\n\n　　当我反应过来的时候，我和H姑娘就成为了朋友，我只记得她的故事让人心疼，我在心中暗自决定，要和她做一辈子的好朋友。至于我们是怎样成为朋友的，我给忘了。人的记忆有限，而我愚笨，记得的东西更有限，反正一去二来，我和H姑娘就成了朋友。说实话，我真怕H姑娘明白了会来揍我。但是就应不会，也许H姑娘已经不在意这些事情了。'),('7b9a3df9530d445c977e1fdec86cd488','22daa83bde614e5bae7000c2d7f5ece7','<p>请输入文本内容</p><pre><code>public static MySqlConnection MySqlConnection<br>        {<br>            get<br>            {<br>                if (_mySqlConnection != null)<br>                {<br>                    return _mySqlConnection;<br>                }<br>                else<br>                {<br>                    if (_mySqlConfig == null)<br>                    {<br>                        InitConfig();<br>                    }<br>                    _mySqlConnection = new MySqlConnection(_mySqlConfig.ConnectStr);<br>                    _mySqlConnection.Open();<br>                    return _mySqlConnection;<br>                   <br>                }<br>            }<br>        }</code></pre><p><br></p>',0,'代码','2019-05-05 18:58:56','请输入内容摘要'),('99fe1dc489d74c31ad5915a95c724362','wCKDOBJI3TP','<p style=\"text-align: center;\">7个葫芦娃</p><p style=\"text-align: center;\">7个小矮人</p><p style=\"text-align: center;\"><img src=\"/editImages/9D6C5E1930D09837838413785AC822714C6D101B.jpg\" style=\"max-width:100%;\"><br></p><div></div><div><p style=\"text-align: center;\"><img src=\"http://localhost:5914/editImages/F3C78672EA24EA7A2F2D99BD16B94B3FDC71D528.jpeg\"><br></p><h1 style=\"text-align: center;\">行宫</h1><p style=\"text-align: center;\"><a href=\"https://www.gushiwen.org/shiwen/default.aspx?cstr=%e5%94%90%e4%bb%a3\">唐代</a>：<a href=\"https://so.gushiwen.org/authorv_201a0677dee4.aspx\">元稹</a></p><div id=\"contson45c396367f59\"><div style=\"text-align: center;\">寥落古行宫，宫花寂寞红。</div><div style=\"text-align: center;\">白头宫女在，闲坐说玄宗。</div></div></div><p style=\"text-align: center;\"><br></p>',0,'7个葫芦娃','2019-03-06 22:18:28','《葫芦兄弟》(又名:葫芦娃)，是上海美术电影制片厂于1986年原创出品的13集系列剪纸动画片，是中国动画第二个繁荣时期的代表作品之一，至今已经成为中国动画经典。\n\n讲述7只神奇的葫芦，7个本领超群的兄弟，为救亲人前赴后继，展开了与妖精们的周旋。《葫芦兄弟》是国内原创经典动画之一 ，该动画自1986年播出以来，一直受到广大观众，尤其是少年儿童们的喜爱。\n\n《葫芦兄弟》其续集《葫芦小金刚》拍摄于1990年前后。'),('aec157d397c0441bb00f84ae9c02b6c8','22daa83bde614e5bae7000c2d7f5ece7','<div label-module=\"para\">中国近代史是从第一次<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E9%B8%A6%E7%89%87%E6%88%98%E4%BA%89\">鸦片战争</a>1840年到1949年南京<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%9B%BD%E6%B0%91%E5%85%9A/226551\">国民党</a>政权迁至台湾、中华人民共和国成立的历史。历经<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E6%B8%85%E7%8E%8B%E6%9C%9D/7966992\">清王朝</a>晚期、<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E4%B8%AD%E5%8D%8E%E6%B0%91%E5%9B%BD%E4%B8%B4%E6%97%B6%E6%94%BF%E5%BA%9C/4484426\">中华民国临时政府</a>时期、<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%8C%97%E6%B4%8B%E5%86%9B%E9%98%80\">北洋军阀</a>时期和<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%9B%BD%E6%B0%91%E6%94%BF%E5%BA%9C/424137\">国民政府</a>时期，是中国<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%8D%8A%E6%AE%96%E6%B0%91%E5%9C%B0%E5%8D%8A%E5%B0%81%E5%BB%BA%E7%A4%BE%E4%BC%9A/788712\">半殖民地半封建社会</a>逐渐形成到<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E7%93%A6%E8%A7%A3\">瓦解</a>的历史。</div><div label-module=\"para\">中国近代史，是一部充满灾难、落后挨打的<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%B1%88%E8%BE%B1/65969\">屈辱</a>史，是一部中国人民探索救国之路，实现自由、民主的探索史，是一部中华民族抵抗侵略，打倒帝国主义以实现民族解放、打倒<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%B0%81%E5%BB%BA%E4%B8%BB%E4%B9%89/590790\">封建主义</a>以实现人民富强的斗争史。</div><div label-module=\"para\">中国近代史可以分为两个阶段。第一个阶段是从1840年鸦片战争到1919年五四运动前夕，是<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E6%97%A7%E6%B0%91%E4%B8%BB%E4%B8%BB%E4%B9%89%E9%9D%A9%E5%91%BD/788911\">旧民主主义革命</a>阶段；第二个阶段是从1919年<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E4%BA%94%E5%9B%9B%E8%BF%90%E5%8A%A8/291670\">五四运动</a>到1949年中华人民共和国成立前夕，是<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E6%96%B0%E6%B0%91%E4%B8%BB%E4%B8%BB%E4%B9%89%E9%9D%A9%E5%91%BD/605010\">新民主主义革命</a>阶段。</div><p><br></p><p><img src=\"https://gss0.bdstatic.com/94o3dSag_xI4khGkpoWK1HF6hhy/baike/c0%3Dbaike92%2C5%2C5%2C92%2C30/sign=ee70e20b007b020818c437b303b099b6/d4628535e5dde71110998958adefce1b9c1661d1.jpg\">&nbsp;&nbsp;<br></p><div label-module=\"para-title\"><h3>步入近代</h3></div><ul><li><div label-module=\"para\"><b>鸦片战争</b></div></li></ul><div label-module=\"para\">鸦片战争前，<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E6%B8%85%E6%9C%9D/175141\">清朝</a>的封建统治已腐朽衰落，国内阶级矛盾、民族矛盾激化，危机重重；而英国则是世界上最强大的资本主义国家。但英货在中国市场上销路不大，这是由于中国自给自足的自然经济对外国商品还具有顽强的抵抗作用。为了改变这种状况，英国殖民主义者大量地向中国推销<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E9%B8%A6%E7%89%87\">鸦片</a>。鸦片贸易侵蚀到天朝官僚体系之心脏、摧毁了宗法制度之堡垒的腐败作用，就是同鸦片烟箱一起从停泊在黄埔的英国趸船上被偷偷带进这个帝国的。”<sup>&nbsp;[1]</sup><a name=\"ref_[1]_5376365\">&nbsp;</a></div><div label-module=\"para\">道光十九年正月（1839年3月），<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E6%9E%97%E5%88%99%E5%BE%90/212\">林则徐</a>到达广<a title=\"第一次鸦片战争\" href=\"https://baike.baidu.com/pic/%E4%B8%AD%E5%9B%BD%E8%BF%91%E4%BB%A3%E5%8F%B2/6067/20452209/0b55b319ebc4b745e5c3d5f1cffc1e178b821585?fr=lemma&amp;ct=cover\" target=\"_blank\" nslog-type=\"10000206\"><div><img alt=\"第一次鸦片战争\" src=\"https://gss2.bdstatic.com/-fo3dSag_xI4khGkpoWK1HF6hhy/baike/s%3D220/sign=6597abd49245d688a702b5a694c37dab/0b55b319ebc4b745e5c3d5f1cffc1e178b821585.jpg\"></div><div>第一次鸦片战争(2张)</div><div><div></div><div></div></div></a>&nbsp;州，并严肃表示禁烟的决心：“若鸦片一日未绝，本大臣一日不回，誓与此事相始终，断无中止之理。”<sup>&nbsp;[2]</sup><a name=\"ref_[2]_5376365\">&nbsp;</a>&nbsp;四月二十二日至五月十三日（6月3日至6月25日），将缴获的鸦片全部在虎门滩当众销毁。<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E8%99%8E%E9%97%A8%E9%94%80%E7%83%9F/719475\">虎门销烟</a>打击了外国侵略者的气焰，鼓舞了中国人民的斗志，向全世界表明了中国人民维护民族尊严和反抗外国侵略的决心。</div><div label-module=\"para\">虎门销烟之后，英国开始对中国发动侵略战争。鸦片战争从道光二十年五月（1840年6月）开始到道光二十二年七月（1842年8月）结束，持续了两年多的时间。1842年8月，英国侵略者又强迫清政府签定了中国近代第一个不平等条约——中英《南京条约》。次年，英国又强迫清政府签定了《<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%8D%97%E4%BA%AC%E6%9D%A1%E7%BA%A6\">南京条约</a>》的附件。鸦片战争刚刚结束，美法两国以武力威胁下，迫使清政府分别和他们签定了不平等的中美《<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E6%9C%9B%E5%8E%A6%E6%9D%A1%E7%BA%A6\">望厦条约</a>》和中法《<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E9%BB%84%E5%9F%94%E6%9D%A1%E7%BA%A6\">黄埔条约</a>》，扩大了侵略权益。</div><div label-module=\"para\">《南京条约》的签订，破坏了中国的领土完整与司法、关税等主权，开创了以条约形式掠夺和奴役中国合法化的先例，中国从封建社会开始沦为半殖民地半封建社会。从此，中华民族与帝国主义、人民大众与封建主义的矛盾，成为中国社会的主要矛盾，反帝反封建成为近代中国人斗争的双重历史任务。中国历史进入了<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E6%97%A7%E6%B0%91%E4%B8%BB%E4%B8%BB%E4%B9%89%E9%9D%A9%E5%91%BD\">旧民主主义革命</a>时期。<sup>&nbsp;[3]</sup><a name=\"ref_[3]_5376365\">&nbsp;</a></div><ul><li><div label-module=\"para\"><b>太平天国起义</b></div></li></ul><div label-module=\"para\"><div><a nslog-type=\"9317\" href=\"https://baike.baidu.com/pic/%E4%B8%AD%E5%9B%BD%E8%BF%91%E4%BB%A3%E5%8F%B2/6067/0/8cb1cb134954092397bd01999258d109b2de4904?fr=lemma&amp;ct=single\" target=\"_blank\" title=\"太平天国运动\"><img src=\"https://gss2.bdstatic.com/9fo3dSag_xI4khGkpoWK1HF6hhy/baike/s%3D220/sign=f85e87750cf431adb8d2443b7b34ac0f/8cb1cb134954092397bd01999258d109b2de4904.jpg\" alt=\"太平天国运动\"></a>太平天国运动</div>鸦片战争中，清政府耗费大量军费，大大加重了人民负担。此后，<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E6%B4%AA%E7%A7%80%E5%85%A8\">洪秀全</a>领导的规模巨大的<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%A4%AA%E5%B9%B3%E5%A4%A9%E5%9B%BD%E8%B5%B7%E4%B9%89\">太平天国起义</a>爆发了。</div><div label-module=\"para\">1853年，太平军占领了南京，定都<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%A4%A9%E4%BA%AC\">天京</a>，太平天国颁布了《<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%A4%A9%E6%9C%9D%E7%94%B0%E4%BA%A9%E5%88%B6%E5%BA%A6\">天朝田亩制度</a>》，另外派了两支人马分头北伐和西征。后来，湘军疯狂反扑，太平军连连失利。1855年，<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E7%9F%B3%E8%BE%BE%E5%BC%80\">石达开</a>指挥西征军大败湘军，太平天国进入军事上的全盛时期，太平天国军事上取得很大胜利的时候，领导人之间，发生了尖锐的内部战争。</div><div label-module=\"para\">1859年，<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E6%B4%AA%E4%BB%81%E7%8E%95/6733987\">洪仁玕</a>提出《<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E8%B5%84%E6%94%BF%E6%96%B0%E7%AF%87/1425401\">资政新篇</a>》，主要内容是：发展工商业、奖励科技发明；开新式学堂；向西方学习，以法治国等。它是先进中国人首次提出在中国发展资本主义的设想，但由于当时形势未能实现。</div><div label-module=\"para\">1864年6月，洪秀全病逝。7月，湘军冲入天京城内，天京陷落，轰轰烈烈的<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%A4%AA%E5%B9%B3%E5%A4%A9%E5%9B%BD%E8%BF%90%E5%8A%A8\">太平天国运动</a>，由于中外反动势力的联合绞杀，而失败了。</div><div label-module=\"para\">太平天国坚持战斗14年，势力发展到18省，是中国近代史上一次伟大的反封建反侵略的农民运动。它建立了政权，颁布了《<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%A4%A9%E6%9C%9D%E7%94%B0%E4%BA%A9%E5%88%B6%E5%BA%A6\">天朝田亩制度</a>》，沉重地打击了中外反动势力，是几千年来中国<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%86%9C%E6%B0%91%E6%88%98%E4%BA%89\">农民战争</a>的最高峰。<sup>&nbsp;[4]</sup><a name=\"ref_[4]_5376365\">&nbsp;</a></div><div><a name=\"2_2\"></a><a name=\"sub5376365_2_2\"></a><a name=\"双半社会的形成\"></a><a name=\"2-2\"></a></div><div label-module=\"para-title\"><h3>双半社会的形成</h3></div><ul><li><div label-module=\"para\"><b>第二次鸦片战争</b></div></li></ul><div label-module=\"para\">1856年，英法借口修约，发动了<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E7%AC%AC%E4%BA%8C%E6%AC%A1%E9%B8%A6%E7%89%87%E6%88%98%E4%BA%89\">第二次鸦片战争</a>。1858年，俄、美、英、法四国先后强迫清政府分别签定<sup>&nbsp;[4]</sup><a name=\"ref_[4]_5376365\">&nbsp;</a>&nbsp;了《<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%A4%A9%E6%B4%A5%E6%9D%A1%E7%BA%A6\">天津条约</a>》。1860年10月下旬，英法两国又强迫清政府签定了中英、中法《<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%8C%97%E4%BA%AC%E6%9D%A1%E7%BA%A6\">北京条约</a>》。</div><div label-module=\"para\"><div><a nslog-type=\"9317\" href=\"https://baike.baidu.com/pic/%E4%B8%AD%E5%9B%BD%E8%BF%91%E4%BB%A3%E5%8F%B2/6067/0/d1a20cf431adcbefa2b2d2e3aaaf2edda2cc9f55?fr=lemma&amp;ct=single\" target=\"_blank\" title=\"第二次鸦片战争-火烧圆明园\"><img src=\"https://gss0.bdstatic.com/-4o3dSag_xI4khGkpoWK1HF6hhy/baike/s%3D220/sign=0371e21e0c46f21fcd345951c6266b31/d1a20cf431adcbefa2b2d2e3aaaf2edda2cc9f55.jpg\" alt=\"第二次鸦片战争-火烧圆明园\"></a>第二次鸦片战争-火烧圆明园</div>咸丰十年（1860）初，英法两国分别任命额尔金和葛罗为全权代表，率舰队前往中国，再次发动侵略战争。六月（7月），英法 联军抵大沽口外。咸丰帝带领一批官员逃往热河（今河北省承德市），令其弟恭亲王奕訢留守北京，负责和议。八月二十二日（10月6日），英法联军占领了清廷经营了150多年的<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%9C%86%E6%98%8E%E5%9B%AD/9328\">圆明园</a>。在被英法联军洗劫一空之后，又被放火烧毁。大火焚烧了三天，号称“万园之园”的圆明园化成了一堆堆败瓦颓垣。参与焚掠的英国殖民主义者戈登（C.G.Gordon）承认：“我们就这样以最野蛮的方式摧毁了世界上最宝贵的财富。<sup>&nbsp;[5]</sup><a name=\"ref_[5]_5376365\">&nbsp;</a>&nbsp;”</div><div label-module=\"para\">经过第二次鸦片战争和不平等的《天津条约》、《北京条约》的签订，中国领土又遭到进一步劫夺，外国侵略者进一步从中国攫得了大量权益，加紧了对中国的政治控制和经济、文化侵略。资本主义各国通过其公使直接向清廷施加压力，操纵、控制中国的内政和外交。大批商埠的增开，从东南沿海一直扩大到沿海七省和长江中游，又使外国资本主义侵略更为深入。外国侵略者还直接管理中国海关，更从财政上加强控制清廷，从而便于扩大其政治影响。中国的主权丧失更多，进一步加深了中国半殖民地化的程度。</div><ul><li><div label-module=\"para\"><b>沙俄侵占中国领土</b></div></li></ul><div label-module=\"para\">俄国一直对中国抱有领土野心。鸦片战争后，它就不断加紧武装侵略中国黑龙江流域。咸丰八年四月十六日（1858年5月28日），即中俄《天津条约》签订前半个月，用武力强迫黑龙江将军奕山签订了不平等的《<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E7%91%B7%E7%8F%B2%E6%9D%A1%E7%BA%A6/359519\">瑷珲条约</a>》。根据这一条约，俄国割占了外兴安岭以南、黑龙江以北60多万平 方公里的中国领土。咸丰十年十月初二日（1860年11月14日），俄国公使伊格那提耶夫逼迫奕?签订不平等的中俄《北京条约》。又把乌苏里江以东约40万平方公里的领土割占了去，还为割占中国西部领土制造了“根据”。</div><div label-module=\"para\">咸丰十一年五月（1861年6月），中俄双方签订了《勘分东界约记》。这次勘界，实际上仅勘分了兴凯湖以南的陆界，并没有勘分乌苏里江和黑龙江的水界。从同治元年五月（1862年6月）起，清廷勘界大臣明谊和俄国全权代表巴布科夫、扎哈罗夫等，在塔尔巴哈台（今新疆塔城）开始勘分西北边界的谈判。通过中俄《北京条约》，俄国强行规定中俄西段边界的走向，把清朝设在境内城镇附近的常住卡伦指为分界标志，把中国的内湖斋桑泊和特穆尔图淖尔（今吉尔吉斯斯坦伊塞克 湖）指为界湖。随后，俄国出兵强占这一地区，制造既成事实。到同治三年九月（1864年10月），中俄双方才在塔城重开谈判。清廷屈于俄国的武力威胁，令明谊让步，接受俄方议单。九月初七日（10月7日），明谊与巴布科夫签订中俄《<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E5%8B%98%E5%88%86%E8%A5%BF%E5%8C%97%E7%95%8C%E7%BA%A6%E8%AE%B0/8844398\">勘分西北界约记</a>》，划定了从沙宾达巴哈山口起至浩罕边境为止的中俄西段边界。</div><div label-module=\"para\">在第二次鸦片战争中，俄国通过不平等的《瑷珲条约》、《北京条约》和一系列勘界条约，侵占了中国144万多平方公里的领土。<sup>&nbsp;[6]</sup><a name=\"ref_[6]_5376365\">&nbsp;</a></div><ul><li><div label-module=\"para\"><b>清朝政局变动</b></div></li></ul><div label-module=\"para\">由于《天津条约》规定外国公使常驻北京，清廷同西方资本主义列强建立正式外交关系已无法避免。咸丰十年十二月（1861年1月），清廷设立了<a target=\"_blank\" href=\"https://baike.baidu.com/item/%E6%80%BB%E7%90%86%E5%90%84%E5%9B%BD%E4%BA%8B%E5%8A%A1%E8%A1%99%E9%97%A8/1200998\">总理各国事务衙门</a>。总理衙门主管外交、通商、关税及建筑铁路、开矿、制造枪炮弹药等事务，总揽了全部洋务事宜。</div><div label-module=\"para\">咸丰十一年七月（1861年8月），咸丰帝在热河病死，其子载淳继位。载淳年幼，遗诏任命怡亲王载垣、郑亲王端华、户部尚书肃顺<div><a nslog-type=\"9317\" href=\"https://baike.baidu.com/pic/%E4%B8%AD%E5%9B%BD%E8%BF%91%E4%BB%A3%E5%8F%B2/6067/0/960a304e251f95ca22621a01c1177f3e660',0,'近代史简介','2019-03-13 17:18:18','中国近代史是从第一次鸦片战争1840年到1949年南京国民党政权迁至台湾、中华人民共和国成立的历史。历经清王朝晚期、中华民国临时政府时期、北洋军阀时期和国民政府时期，是中国半殖民地半封建社会逐渐形成到瓦解的历史。\n中国近代史，是一部充满灾难、落后挨打的屈辱史，是一部中国人民探索救国之路，实现自由、民主的探索史，是一部中华民族抵抗侵略，打倒帝国主义以实现民族解放、打倒封建主义以实现人民富强的斗争史。\n中国近代史可以分为两个阶段。第一个阶段是从1840年鸦片战争到1919年五四运动前夕，是旧民主主义革命阶段；第二个阶段是从1919年五四运动到1949年中华人民共和国成立前夕，是新民主主义革命阶段。'),('e58444852cda405f87424bfc0dbf6a70','de08bf7dfd1740579a55e071a403d608','<p><img src=\"/editImages/5A6FFB30B192718ED45CCEF724423A86F6E5806B.png\" style=\"max-width:100%;\"><br></p><p>s</p>',0,'表示','2019-04-10 15:25:05','请输入内容摘要');
/*!40000 ALTER TABLE `chapter` ENABLE KEYS */;

#
# Structure for table "group"
#

DROP TABLE IF EXISTS `group`;
CREATE TABLE `group` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL DEFAULT '',
  `root` bit(1) DEFAULT b'0' COMMENT '拥有超级权限',
  `admin` bit(1) DEFAULT b'0' COMMENT '拥有管理员权限',
  `uploadPic` bit(1) DEFAULT b'0' COMMENT '允许上传漫画',
  `deletePic` bit(1) DEFAULT b'0' COMMENT '允许上传漫画',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

#
# Data for table "group"
#

/*!40000 ALTER TABLE `group` DISABLE KEYS */;
/*!40000 ALTER TABLE `group` ENABLE KEYS */;

#
# Structure for table "pay"
#

DROP TABLE IF EXISTS `pay`;
CREATE TABLE `pay` (
  `Id` varchar(255) NOT NULL DEFAULT '',
  `UserId` varchar(255) DEFAULT '0' COMMENT '用户ID',
  `Type` varchar(255) DEFAULT NULL COMMENT '购买内容',
  `Chapter` varchar(255) DEFAULT NULL,
  `PayTime` datetime DEFAULT NULL COMMENT '购买日期',
  `Money` int(11) NOT NULL DEFAULT '0' COMMENT '消费',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

#
# Data for table "pay"
#

/*!40000 ALTER TABLE `pay` DISABLE KEYS */;
INSERT INTO `pay` VALUES ('98ed5a08022f4104bd32a2a57ec36be9','1b3e910e-1630-4554-87af-4304d2fb8cfa','0','7b9a3df9530d445c977e1fdec86cd488','2019-05-05 18:59:04',12),('aa692ca3d4884de58752f04befca6ed6','1b3e910e-1630-4554-87af-4304d2fb8cfa','0','3b2656d04a1d45c0ad158fffef422e0f','2019-05-01 17:32:57',12),('c5fd3f185d39434e9457e68ccc769c13','1b3e910e-1630-4554-87af-4304d2fb8cfa','0','e58444852cda405f87424bfc0dbf6a70','2019-04-10 15:25:59',12),('ca76e97699de42d5a0b3e4f2cf1cb4d0','1b3e910e-1630-4554-87af-4304d2fb8cfa','0','99fe1dc489d74c31ad5915a95c724362','2019-03-20 11:06:33',12),('FtKZ9PirNC&amp;IlHE1','FtKZ9PirNC&amp;IlHE1','123','FtKZ9PirNC&amp;IlHE1','2018-12-13 00:00:00',2);
/*!40000 ALTER TABLE `pay` ENABLE KEYS */;

#
# Structure for table "user"
#

DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `userid` varchar(255) NOT NULL DEFAULT '0',
  `username` varchar(255) NOT NULL DEFAULT '飞扬',
  `email` varchar(255) NOT NULL DEFAULT '',
  `password` varchar(255) NOT NULL DEFAULT '',
  `age` int(11) DEFAULT '0',
  `mygroup` varchar(255) NOT NULL DEFAULT 'guest' COMMENT '用户组',
  `intro` varchar(255) DEFAULT NULL COMMENT '用户备注',
  `address` varchar(255) DEFAULT NULL COMMENT '地址',
  `activity` varchar(255) NOT NULL DEFAULT '0' COMMENT '已经激活',
  `token` varchar(255) NOT NULL DEFAULT '0' COMMENT '授权',
  `telephone` varchar(255) DEFAULT '0' COMMENT '电话',
  `qq` varchar(255) DEFAULT '0' COMMENT '腾讯QQ',
  `twitter` varchar(255) DEFAULT '0' COMMENT '微博',
  `avatar` varchar(255) DEFAULT 'no.jpg' COMMENT '头像',
  `rank` int(11) DEFAULT '0' COMMENT '用户等级',
  `credit` int(11) DEFAULT '0' COMMENT '信用',
  `gb` int(11) NOT NULL DEFAULT '0' COMMENT '积分',
  PRIMARY KEY (`userid`),
  KEY `email` (`email`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

#
# Data for table "user"
#

/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES ('1b3e910e-1630-4554-87af-4304d2fb8cfa','王咸强','1757696115@qq.com','9%B3,qYH>.ZY4Y-7',1,'user','hello world','未选择','0','ZDgbqPpzjFcWPvh4SKGMbJeDsfHwVjgm3HKgja6iCxVdTGgLyla0JGVvVsiHNgx0JKrba3sASZtcE5Y1jx30JXqjh2QtxFiKsyr0J2bab2AL7WGpHqp0J9FGuhfRnjC4&success=success&userid=1b3e910e-1630-4554-87af-4304d2fb8cfa','0','0','0','1b3e910e-1630-4554-87af-4304d2fb8cfa.jpg',0,0,79),('5491dc33-5e0f-4957-96d9-b8af8e471963','outlook','huanying521@outlook.com','4641',1,'user','hello world','未选择','1','iUQplawzx5QPTzKiPKriKPV9Cq7Zgambx6mu5V7QFZiMHVLjwfG5EhZwYJdnPNuMXz8F49uwAESDjMpa7Vp1SaPZM5rPbKBUsHVgkyt7ev1zwU4Vzbtw2LWGmztqzrzD','0','0','0','no.jpg',0,0,0);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
