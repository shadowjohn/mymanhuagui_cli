# -*- coding: utf-8 -*-
# From : https://github.com/wzdlc1996/mangaSpider
# From : https://wzdlc1996.github.io/artic/other/mangaspider/
# Major Author : wzdlc1996
# Author : 羽山 https://3wa.tw/
# Version : 2020-11-06
import urllib
import dukpy
import lzstring
import re
import requests
import bs4
import json
import sys
UA = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:70.0) Gecko/20100101 Firefox/70.0"
baseURL = "https://www.manhuagui.com"
imgURL = "https://i.hamreus.com"
url = "https://www.manhuagui.com/comic/7580/523083.html"
if len(sys.argv)!=2:
  print("\nUsage : get_manhuagui.exe \"https://www.manhuagui.com/comic/7580/523083.html\" \n")
  print("\nUsage : get_manhuagui.exe test\n")
  sys.exit()
if sys.argv[1]!="test":
  url = sys.argv[1];   
'''
def getChapterList(url):
    global UA
    #with open("./config.json",'r') as f:
    #    conf = json.load(f)
    baseSite = requests.get(url, headers = {"User-Agent": UA})
    formatedSite = bs4.BeautifulSoup(baseSite.content, features="lxml")
    allChapLis = formatedSite.findAll(id="chapter-list-1")
    res = []
    for div in allChapLis:
        allLinks = div.findAll('a')
        for aTags in allLinks:
            det = aTags.attrs
            res.append({"Text": aTags.getText(), "URL":baseURL+det['href']})
    return res
'''    
def getCoreInfo(url):
    #with open("./config.json", "r") as f:
        #conf = json.load(f)
    global UA
    getSite = bs4.BeautifulSoup(requests.get(url, headers = {"User-Agent": UA}).content, features="lxml")
    jsSlic = re.search(r">window.*(\(function\(p.*?)</script>", str(getSite)).group(1)
    coreStr = re.search(r"[0-9],'([A-Za-z0-9+/=]+?)'", jsSlic).group(1)
    decStr = lzstring.LZString.decompressFromBase64(coreStr)
    jsNew = re.sub(r"'[A-Za-z0-9+/=]*'\[.*\]\('\\x7c'\)", "'"+decStr+"'.split('|')", jsSlic)
    sol = dukpy.evaljs(jsNew)
    return json.loads(re.search(r"(\{.*\})",sol).group(1))
'''
def getDlSetting(url):
    data = getCoreInfo(url)
    pathURLs = []
    picid = 0
    for pic in data['files']:
        mangaurl = imgURL+data['path']+re.match(r".*?\.[a-z]*", pic).group(0)
        fullurl = mangaurl+"?cid="+str(data['cid'])+"&md5="+data['sl']['md5']
        pathURLs.append({"Name": "%05d"%picid, "URL": fullurl})
        picid += 1
    refURL = baseURL+"/comic/"+str(data['bid'])+"/"+str(data['cid'])+".html"
    return pathURLs, {"Referer": refURL}    
'''
#print(getChapterList(url))
#print(getCoreInfo(url))
#https://i.hamreus.com/ps3/y/yiquanchaoren/第179话/0TIG5n.png.webp?e=1605310131&m=vzvYBcVxGKB-CsJzX5-XDQ

DS = getCoreInfo(url) 
sys.stdout.write(json.dumps(DS, indent=4))
sys.stdout.write("\n########################################\n")
for i in DS['files']:
  p = DS["path"]
  p = DS["path"].replace(DS["cname"],urllib.parse.quote(DS["cname"]))
  url = imgURL+str(p)+str(i)+"?e="+str(DS["sl"]["e"])+"&m="+str(DS["sl"]["m"])
  sys.stdout.write(url+"\n")
sys.exit(0)  