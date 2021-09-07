using JiebaNet.Segmenter;
using MoreNote.Common.Utils;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.Segmenter
{
    public class JiebaSegmenterService
    {
        public NpgsqlTsVector GetNpgsqlTsVector(string content)
        {
            NpgsqlTsVector vector;
        
            try
            {
                var segmenter = new JiebaSegmenter();
                HtmlToTextHelper htmlToTextHelper = new HtmlToTextHelper();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return null; ;
                }
                string noHtmlConent = htmlToTextHelper.Convert(content);
                var list = segmenter.Cut(noHtmlConent, hmm: true);
                var cutList = new List<string>();
                foreach (var item in list)
                {
                    if (item.Length > 1)
                    {
                        cutList.Add(item.ToUpper());
                    }

                }
                string str = string.Join(" ", cutList);
                vector = NpgsqlTsVector.Parse(str);
            }
            catch (Exception ex)
            {
                return null;
            }

            return vector;
        }

        public string GetSearchKeyWorlds(string keyword)
        {
            var segmenter = new JiebaSegmenter();
            var list = segmenter.CutForSearch(keyword, hmm: true);
            string result = string.Join(" ", list);
            return result;
        }
        public NpgsqlTsQuery GetSerachNpgsqlTsQuery(string keyword)
        {
            NpgsqlTsQuery vector;
             try
            {
                var segmenter = new JiebaSegmenter();
                HtmlToTextHelper htmlToTextHelper = new HtmlToTextHelper();
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return null; ;
                }
                string noHtmlConent = htmlToTextHelper.Convert(keyword);
                var list = segmenter.CutForSearch(noHtmlConent, hmm: true);
                var cutList = new List<string>();
                foreach (var item in list)
                {
                    if (item.Length > 1)
                    {
                        cutList.Add(item.ToUpper());
                    }
                }
                string str = string.Join(" & ", cutList);
                vector = NpgsqlTsQuery.Parse(str);
                
            }
            catch (Exception ex)
            {
                return null;
            }

            return vector;
        }
    }
}