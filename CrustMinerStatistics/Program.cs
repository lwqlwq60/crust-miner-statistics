using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using NpoiMapper;

namespace CrustStatistics
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            if (!Directory.Exists("./导出"))
            {
                Directory.CreateDirectory("./导出");
            }

            var owner = File.ReadAllLines("owner.txt")[0];
            Console.WriteLine("Owner地址：" + owner);
            Console.WriteLine("开始统计........");
            var client = new HttpClient();

            var memberRequest = new MemberRequest
            {
                GroupOwner = owner
            };
            var memberRes = await client.PostAsJsonAsync(Endpoints.GetMember, memberRequest);
            var member = await memberRes.Content.ReadFromJsonAsync<MemberResponse>();
            var onwers = File.ReadAllLines("members.txt");
            var ownerDict = new Dictionary<string, string>();
            foreach (var onwer in onwers)
            {
                if (!string.IsNullOrWhiteSpace(onwer))
                {
                    var splits = onwer.Split(' ');
                    ownerDict.Add(splits[0], splits[1]);
                }
            }

            if (member?.Code == 0)
            {
                Console.WriteLine($"获取到{member.Data.Count}个member.");
                var list = member.Data.List;
                foreach (var data in list)
                    if (ownerDict.TryGetValue(data.AccountId, out var value))
                        data.Owner = value;

                var excel = new ExportMapper<MemberData>();
                excel.Map(0, _ => _.Owner, "矿工")
                    .Map(1, _ => _.AccountId, "节点地址")
                    .Map(2, _ => Math.Round(0.90949227 * ((double)_.Capacity / 1000000000000.0), 3),
                        "SubScan显示存储空间(TiB)")
                    .Map(3, _ => Math.Round((double)_.Capacity / 1000000000000.0, 5), "上报存储空间(TiB)");

                var file = $"./导出/节点统计{DateTime.Now.ToString("yyyyMMddHHmm")}.xls ";
                excel.Save(file, list, $"节点统计 {DateTime.Now.ToString("yyyy-MM-dd")}");
                Console.WriteLine("统计member成功.");

                var rewardRes =
                    await client.PostAsJsonAsync(Endpoints.GetReward, new RewardRequest { Address = owner });
                var rewards = await rewardRes.Content.ReadFromJsonAsync<RewardResponse>();

                if (rewards?.Code == 0)
                {
                    Console.WriteLine($"获取到上{rewards.Data.Count}条收益.");
                    var rewardExcel = new ExportMapper<Reward>();
                    rewardExcel.Map(0, _ => _.BlockNum, "区块")
                        .Map(1, _ => _.EventIndex, "事件ID").Map(2, _ =>
                        {
                            var dt = new DateTime(1970, 1, 1);
                            return dt.AddSeconds(_.BlockTimestamp).ToString("yyyy-MM-dd HH:mm:ss");
                        }, "时间").Map(3, _ =>
                        {
                            var pairs = JsonSerializer.Deserialize<IEnumerable<Pair>>(_.Params);
                            var balance = pairs?.FirstOrDefault(__ => __.Type == "Balance");
                            if (balance != null)
                                return Math.Round((double)ulong.Parse(balance.Value) / 1000000000000.0, 15);

                            return "无法获取";
                        }, "收益(CRU)");
                    var rewardFile = $"./导出/收益历史{DateTime.Now.ToString("yyyyMMddHHmm")}.xls ";
                    rewardExcel.Save(rewardFile, rewards.Data.List,
                        $"收益历史 {DateTime.Now.ToString("yyyy-MM-dd")}）");
                    Console.WriteLine("统计收益成功.");
                }
            }

            Console.WriteLine("统计结束.按任意键推出.");
            Console.ReadKey();
        }
    }
}