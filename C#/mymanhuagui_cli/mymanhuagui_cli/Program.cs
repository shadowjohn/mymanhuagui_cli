using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using utility;
using LZStringCSharp;
using OdeToCode.Utility;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web;

namespace mymanhuagui_cli
{
    class Program
    {
        static string URL = "https://tw.manhuagui.com/comic/7580/523083.html";
        static string MESSAGE = @"
Usage :
    mymanhuagui_cli.exe ""URL""
    mymanhuagui_cli.exe test
    mymanhuagui_cli.exe """ + URL + @"""    
";
        static public void echo(string data)
        {
            Console.WriteLine(data);
        }
        static public void exit()
        {
            Environment.Exit(1);
        }
        //From : https://weblog.west-wind.com/posts/2007/feb/14/evaluating-javascript-code-from-c
        public static Microsoft.JScript.Vsa.VsaEngine Engine = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
        public static object EvalJScript(string JScript)
        {
            object Result = null;
            try
            {
                Result = Microsoft.JScript.Eval.JScriptEvaluate(JScript, Engine);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return Result;
        }
        static myinclude my = new myinclude();
        static void Main(string[] args)
        {
            if (args.Count() < 1)
            {
                echo(MESSAGE);
                exit();
            }
            if (my.is_string_like(args[0], "https://") || my.is_string_like(args[0], "http://"))
            {
                URL = args[0];
            }
            if (!my.is_string_like(URL, "https://") && !my.is_string_like(URL, "http://"))
            {
                echo(MESSAGE);
                exit();
            }

            string data = my.b2s(my.file_get_contents(URL));
            //1.抓出 【(function(】 ~ 【{}))】
            string orin_scripts = "(function(" + my.get_between(data, "(function(", "{}))") + "{}))";
            //echo(orin_scripts);
            //exit();
            //2.抓出最後一組的 【,'】 ~ 【'[】
            var m = my.explode(",'", orin_scripts);
            string _sc = my.get_between(m[m.Count() - 1], "", "'[");
            //echo(_sc);
            //exit();
            //3.解碼
            string _c = LZString.DecompressFromBase64(_sc);
            //echo(_c);
            //4.把 第3步解碼的內容 _c ，蓋回 orin_scrtips 的 _sc 的內容
            string new_scripts = orin_scripts.Replace(_sc, _c);
            //echo(new_scripts);
            //5.把 【['\x73\x70\x6c\x69\x63']('\x7c')】，換成【.split('|')】
            new_scripts = new_scripts.Replace(@"['\x73\x70\x6c\x69\x63']('\x7c')", ".split('|')");
            //echo(new_scripts);
            //exit();
            //6. script 最後加上 ;
            new_scripts += ";";
            //echo(new_scripts);
            //exit();
            //7. eval script 可得
            /*
             (function(p,a,c,k,e,d){e=function(c){return(c<a?"":e(parseInt(c/a)))+((c=c%a)>35?String.fromCharCode(c+29):c.toString(36))};if(!''.replace(/^/,String)){while(c--)d[e(c)]=k[c]||e(c);k=[function(e){return d[e]}];e=function(){return'\\w+'};c=1;};while(c--)if(k[c])p=p.replace(new RegExp('\\b'+e(c)+'\\b','g'),k[c]);return p;}('L.t({"p":4,"o":"n","l":"4.k","j":i,"h":"5","g":["f.2.3","q.2.3","d.2.3","c.2.3","b.2.3","a.2.3","9.2.3","8.2.3","7.2.3","6.2.3","r.2.3","B.2.3"],"J":I,"H":G,"F":"/E/y/K/5/","C":1,"D":"","A":0,"z":x,"w":{"e":v,"m":"u"}}).s();',48,48,'||png|webp|7580|第179?|0TIo6h|0TIVsX|0TIKN8|0TI7Rf|0TI6C6|0TIXGi|0TIkQg|0TIbMu||0TIG5n|files|cname|523083|cid|jpg|bpic||一拳超人|bname|bid|0TIZSs|0TIRM2|preInit|imgData|P4bLkJTpCAo5SRkd480gag|1606519731|sl|522380||prevId|nextId|0TIYU9|status|block_cc|ps3|path|12|len|false|finished|yiquanchaoren|SMH'.split('|'),0,{}));

             "SMH.imgData({"bid":7580,"bname":"一拳超人","bpic":"7580.jpg","cid":523083,"cname":"第179话","files":["0TIG5n.png.webp","0TIZSs.png.webp","0TIbMu.png.webp","0TIkQg.png.webp","0TIXGi.png.webp","0TI6C6.png.webp","0TI7Rf.png.webp","0TIKN8.png.webp","0TIVsX.png.webp","0TIo6h.png.webp","0TIRM2.png.webp","0TIYU9.png.webp"],"finished":false,"len":12,"path":"/ps3/y/yiquanchaoren/第179?/","status":1,"block_cc":"","nextId":0,"prevId":522380,"sl":{"e":1606519731,"m":"P4bLkJTpCAo5SRkd480gag"}}).preInit();"              
            */
            string finalScripts = EvalJScript(new_scripts).ToString();
            //8.取得中間 json 的內容 【SMH.imgData(】~ 【).preInit();】
            finalScripts = my.get_between(finalScripts, "SMH.imgData(", ").preInit();");
            //echo(finalScripts);
            //9.反序列化，加入一個新的 path_encode
            //From : https://ithelp.ithome.com.tw/articles/10194853
            /*
{
  "bid": 7580,
  "bname": "一拳超人",
  "bpic": "7580.jpg",
  "cid": 523083,
  "cname": "第179?",
  "files": [
    "0TIG5n.png.webp",
    "0TIZSs.png.webp",
    "0TIbMu.png.webp",
    "0TIkQg.png.webp",
    "0TIXGi.png.webp",
    "0TI6C6.png.webp",
    "0TI7Rf.png.webp",
    "0TIKN8.png.webp",
    "0TIVsX.png.webp",
    "0TIo6h.png.webp",
    "0TIRM2.png.webp",
    "0TIYU9.png.webp"
  ],
  "finished": false,
  "len": 12,
  "path": "/ps3/y/yiquanchaoren/第179?/",
  "status": 1,
  "block_cc": "",
  "nextId": 0,
  "prevId": 522380,
  "sl": {
    "e": 1606519731,
    "m": "P4bLkJTpCAo5SRkd480gag"
  }
}             
            */
            //finalScripts
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(finalScripts));
            DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(myStruct));
            myStruct model = (myStruct)deseralizer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            model.path_encode = HttpUtility.UrlEncode(model.path).Replace("%2f", "/");

            //10.重新序列化，加入一個新的 
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(myStruct));
            MemoryStream msObj = new MemoryStream();
            //將序列化之後的Json格式資料寫入流中
            js.WriteObject(msObj, model);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj, Encoding.UTF8);
            finalScripts = sr.ReadToEnd();
            sr.Close();
            msObj.Close();
            //echo(finalScripts);

            //最後結果
            string output = "";
            output += finalScripts;
            output += "\r\n###################################\r\n";
            //有幾個 files ，合併網址
            string imgURL = "https://i.hamreus.com";
            for (int i = 0, max_i = model.files.Count; i < max_i; i++)
            {
                string _URL = imgURL + model.path_encode + model.files[i] + "?e=" + model.sl.e.ToString() + "&m=" + model.sl.m;
                output += _URL + "\r\n";
            }
            output = output.Trim();
            echo(output);

        }
    }
}
