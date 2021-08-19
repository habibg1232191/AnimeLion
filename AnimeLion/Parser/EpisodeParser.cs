using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AnimeLion.Parser.Models;
using Newtonsoft.Json;

namespace AnimeLion.Parser
{
    public class EpisodeParser
    {
        public int EpisodeCount { get; set; }
        public string LinkParse { get; }

        public EpisodeParser(string url, int episodeCount)
        {
            LinkParse = url;
            EpisodeCount = episodeCount;
        }

        public async Task<VideoInfo> GetVideoInfo()
        {
            var pd = ParametersToDictionary(GetParameters(LinkParse));
            var response = await Post("http:" + GetAddress(LinkParse), pd);
            var videoInfoParameter = GetInfoParameterFromJsCode(response);
            
            var requestDictionary = new Dictionary<string, string>
            {
                {"d", pd["d"]},
                {"d_sign", pd["d_sign"]},
                {"pd", pd["pd"]},
                {"pd_sign", pd["pd_sign"]},
                {"ref", pd["ref"]},
                {"ref_sign", pd["ref_sign"]},
                {"bad_user", "false"},
                {"hash2", "vbWENyTwIn8I"},
                {"type", videoInfoParameter["type"]},
                {"hash", videoInfoParameter["hash"]},
                {"id", videoInfoParameter["id"]},
                {"info", "{\"advImps\":{}}"},
            };
            
            string getVideoInfo = "https://aniqit.com/get-video-info";
            var getVideo = await Post(getVideoInfo, requestDictionary);
            var videoInfo = JsonConvert.DeserializeObject<VideoInfo>(getVideo)!;
            videoInfo.EpisodeNum = EpisodeCount;
            return videoInfo;
        }
        
        private async Task<string> Post(string url, Dictionary<string, string> data)
        {
            HttpClient httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64;" +
                " x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.106 Safari/537.36"
                );
            var parameters = data;
            var encodedContent = new FormUrlEncodedContent(parameters!);
            
            var response = await httpclient.PostAsync(url, encodedContent).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private string GetAddress(string str)
        {
            string result = "";
            bool isComp = true;
            foreach (var chars in str)
            {
                if (chars == '?') isComp = false;
                if (isComp) result += chars;
            }
            return result;
        }
        
        private Dictionary<string, string> ParametersToDictionary(string parameters)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            List<string> parameterList = new List<string>();
            var center = "";
            
            foreach (var parameter in parameters)
            {
                if (parameter == '&')
                {
                    parameterList.Add(center);
                    center = "";
                    continue;
                }
                center += parameter;
            }

            var equally = "";
            var p = "";
            bool isP = false;
            foreach (var parameter in parameterList)
            {
                foreach (var c in parameter)
                {
                    if (c == '=')
                    {
                        isP = true;
                        continue;
                    }

                    if (isP)
                        p += c;
                    else
                        equally += c;
                }
                result.Add(equally, p);
                equally = "";
                p = "";
                isP = false;
            }
            return result;
        }

        private string GetParameters(string str)
        {
            string result = "";
            bool isComp = false;
            foreach (var chars in str)
            {
                if (isComp) result += chars;
                if (chars == '?') isComp = true;
            }
            return result;
        }
        
        private Dictionary<string, string> GetInfoParameterFromJsCode(string jsCode)
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                {"type", GetParameterFromJs(jsCode, "videoInfo.type")},
                {"hash", GetParameterFromJs(jsCode, "videoInfo.hash")},
                {"id", GetParameterFromJs(jsCode, "videoInfo.id")}
            };

            return result;
        }

        private string GetParameterFromJs(string jsCode, string parameterSearch)
        {
            int start = jsCode.IndexOf(parameterSearch, 0, StringComparison.Ordinal) + parameterSearch.Length;

            string parameter = "";
            bool isContinue = false;
            for (int i = start;; i++)
            {
                if (isContinue && jsCode[i] == '\'')
                {
                    break;
                }
                if (isContinue)
                    parameter += jsCode[i];
                if (jsCode[i] == '\'')
                    isContinue = true;
            }

            return parameter;
        }

        public override string ToString()
        {
            return $"EpisodeCount: {EpisodeCount}, LinkParse: {LinkParse}";
        }
    }
}