using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        string text = 
  @"[2018-07-06 09:31:04,230:][INFO ][T44] [缓存|FuturePosition]收到空的更新消息包：行={0}, 列={40}
    [2018-07-06 09:31:04,231:][INFO ][T42] [缓存|期货委托表]变更通知成功：耗时=0,Type=FutureRevertPositionService,Method=FutureEntrust_AsyncChanged,opType=UPDATE,entity=427|674|1077|9|10|190003c13c2|9|5|3|rb1810|0|3680|32|2|30|30|0|0|0|210027| |344|0|1467|a|0|0|6|0|0|6|02309944|30158885|20033|B|0|0|64282|1| |0001-01-01 00:00:00|2018-07-06 09:31:04|0
    [2018-07-06 09:31:04,231:][INFO ][T42] [缓存|期货委托表]变更通知成功：耗时=0,Type=OCacheViewController`2,Method=cacheTable_Changed,opType=UPDATE,entity=427|674|1077|9|10|190003c13c2|9|5|3|rb1810|0|3680|32|2|30|30|0|0|0|210027| |344|0|1467|a|0|0|6|0|0|6|02309944|30158885|20033|B|0|0|64282|1| |0001-01-01 00:00:00|2018-07-06 09:31:04|0
    [2018-07-06 09:31:04,232:][INFO ][T42] [缓存|期货委托表]变更通知成功：耗时=0,Type=OCacheViewController`2,Method=cacheTable_Changed,opType=UPDATE,entity=427|674|1077|9|10|190003c13c2|9|5|3|rb1810|0|3680|32|2|30|30|0|0|0|210027| |344|0|1467|a|0|0|6|0|0|6|02309944|30158885|20033|B|0|0|64282|1| |0001-01-01 00:00:00|2018-07-06 09:31:04|0
    [2018-07-06 09:31:04,232:][INFO ][T42] [缓存|FutureEntrust]更新：writeRedo={True}, merged={True}, 原记录={427|674|1077|9|10|190003c13c2|9|5|3|rb1810|0|3680|32|2|30|30|0|0|0|210027| |344|0|1467|a|0|0|6|0|0|6|02309944|30158885|20033|B|0|0|64282|1| |0001-01-01 00:00:00|2018-07-06 09:31:04|0}, 待更新记录={427|674|1077|9|10|190003c13c2|9|5|3|rb1810|0|3680|32|2|30|30|0|0|0|210027| |344|0|1467|a|0|0|6|0|0|6|02309944|30158885|20033|B|0|0|64282|1| |0001-01-01 00:00:00|0001-01-01 00:00:00|0}
    [2018-07-06 09:31:04,232:][INFO ][T42] [缓存|期货委托表]变更通知成功：耗时=0,Type=FutureRevertPositionService,Method=FutureEntrust_AsyncChanged,opType=INSERT,entity=427|674|1077|F|222|190003c13c2|9|5|3|rb1810|0|3680|32|2|30|0|0|0|0|210027| |344|0|1467|a|0|0|6|0|0|91|02309944|30158885|20033|B|10|0|64282|1| |0001-01-01 00:00:00|2018-07-06 09:31:04|0
    [2018-07-06 09:31:04,233:][INFO ][T42] [缓存|期货委托表]变更通知成功：耗时=0,Type=OCacheViewController`2,Method=cacheTable_Changed,opType=INSERT,entity=427|674|1077|F|222|190003c13c2|9|5|3|rb1810|0|3680|32|2|30|0|0|0|0|210027| |344|0|1467|a|0|0|6|0|0|91|02309944|30158885|20033|B|10|0|64282|1| |0001-01-01 00:00:00|2018-07-06 09:31:04|0
    [2018-07-06 09:31:04,233:][INFO ][T42] [缓存|期货委托表]变更通知成功：耗时=0,Type=OCacheViewController`2,Method=cacheTable_Changed,opType=INSERT,entity=427|674|1077|F|222|190003c13c2|9|5|3|rb1810|0|3680|32|2|30|0|0|0|0|210027| |344|0|1467|a|0|0|6|0|0|91|02309944|30158885|20033|B|10|0|64282|1| |0001-01-01 00:00:00|2018-07-06 09:31:04|0
    [2018-07-06 09:31:04,233:][INFO ][T42] [缓存|FutureEntrust]收到消息并更新成功：主题={oplus.jy_tfutentrusts}
    [2018-07-06 09:31:04,407:][INFO ][T43] [缓存|FuturePre]收到更新消息(FuturePre,oplus.jy_tfutadvscheme)，合计1行62列：
    第一行：adv_serial_no=344,adv_batch_serial_no=4,adv_type=2,fund_id=427,asset_id=674,combi_id=1077,market_no=3,report_code=rb1810,firstleg_reportcode=,firstleg_marketno=0,secondleg_reportcode=,secondleg_marketno=0,entrust_direction=32,entrust_price_type= ,entrust_price=3680.000000000000,entrust_amount=30.0000,entrusted_amount=0.0000,dealed_amount=0.0000,pre_adv_time=205958,quote_price_type= ,trigger_price_type= ,compare_direction=0,trigger_price=0.000000000000,trigger_time=0,stop_order_float_value=0.000000000000,algo_entrust_price_type=2,algo_entrust_price_interval=0,scheme_status=b,ins_id=0,warn_message=,remark=[方案已完成];,operator_no=1467,stop_profit_quote_price_type= ,stop_loss_quote_price_type= ,stop_profit_price=0.000000000000,stop_loss_price=0.000000000000,scheme_code=344,invest_type=a,trigger_price_interval=0.000000000000,firstleg_deal_amount=0.0000,secondleg_deal_amount=0.0000,firstleg_ratio=0.0000,secondleg_ratio=0.0000,cmborder_type=0,valid_time_type=1,pre_adv_date=20180705,firstleg_ratio_monitor=0.0000,secondleg_ratio_monitor=0.0000,trade_interface_type=6,entrust_serial_no=0,trigger_mode=0,max_withdraw_times=0,cur_withdraw_times=1,close_direction=0,firstleg_deal_avg_price=0.000000000000,secondleg_deal_avg_price=0.000000000000,firstleg_deal_balance=0.00,secondleg_deal_balance=0.00,entrust_amount_allowance=0.0000,open_amount_limit=0,risk_warning_flag=0,cache_valid_flag=1

    [2018-07-06 09:31:04,409:][INFO ][T43] [缓存|期货埋单表]变更通知成功：耗时=0,Type=FutureLocalAdvMonitorService,Method=FuturePre_AsyncChanged,opType=UPDATE,entity=344|4|2|427|674|1077|3|rb1810||0||0|32| |3680|30|0|0|205958| | |0|0|0|0|2|0|b|0||[方案已完成];|1467| | |0|0|344|a|0|0|0|0|0|0|1|20180705|0|0|6|0|0|0|1|0|0|0|0|0|0|0|0001-01-01 00:00:00|2018-07-06 09:31:04|0
    [2018-07-06 09:31:04,409:][INFO ][T43] [缓存|期货埋单表]变更通知成功：耗时=0,Type=OCacheViewController`2,Method=cacheTable_Changed,opType=UPDATE,entity=344|4|2|427|674|1077|3|rb1810||0||0|32| |3680|30|0|0|205958| | |0|0|0|0|2|0|b|0||[方案已完成];|1467| | |0|0|344|a|0|0|0|0|0|0|1|20180705|0|0|6|0|0|0|1|0|0|0|0|0|0|0|0001-01-01 00:00:00|2018-07-06 09:31:04|0
    [2018-07-06 09:31:04,409:][INFO ][T43] [缓存|期货埋单表]变更通知成功：耗时=0,Type=OCacheViewController`2,Method=cacheTable_Changed,opType=UPDATE,entity=344|4|2|427|674|1077|3|rb1810||0||0|32| |3680|30|0|0|205958| | |0|0|0|0|2|0|b|0||[方案已完成];|1467| | |0|0|344|a|0|0|0|0|0|0|1|20180705|0|0|6|0|0|0|1|0|0|0|0|0|0|0|0001-01-01 00:00:00|2018-07-06 09:31:04|0
    [2018-07-06 09:31:04,410:][INFO ][T43] [缓存|FuturePre]更新：writeRedo={True}, merged={True}, 原记录={344|4|2|427|674|1077|3|rb1810||0||0|32| |3680|30|0|0|205958| | |0|0|0|0|2|0|b|0||[方案已完成];|1467| | |0|0|344|a|0|0|0|0|0|0|1|20180705|0|0|6|0|0|0|1|0|0|0|0|0|0|0|0001-01-01 00:00:00|2018-07-06 09:31:04|0}, 待更新记录={344|4|2|427|674|1077|3|rb1810||0||0|32| |3680|30|0|0|205958| | |0|0|0|0|2|0|b|0||[方案已完成];|1467| | |0|0|344|a|0|0|0|0|0|0|1|20180705|0|0|6|0|0|0|1|0|0|0|0|0|0|0|0001-01-01 00:00:00|0001-01-01 00:00:00|0}
    [2018-07-05 14:57:53,443:][INFO ][T44] [缓存|期货保证金优惠明细表]变更通知成功：耗时=0,Type=FutureCombiPosPresenter,Method=FutureEnableDetail_AsyncChanged,opType=UPDATE,entity=513|800|824|9|pp1809|b|2|1|30350435|20180611|0|0|9389|0001-01-01 00:00:00|2018-07-05 14:57:53|0
    [2018-07-05 14:57:53,443:][INFO ][T44] [缓存|FutureEnableDetail]更新：writeRedo={True}, merged={True}, 原记录={513|800|824|9|pp1809|b|2|1|30350435|20180611|0|0|9389|0001-01-01 00:00:00|2018-07-05 14:57:53|0}, 待更新记录={513|800|824|9|pp1809|b|2|1|30350435|20180611|0|0|9389|0001-01-01 00:00:00|0001-01-01 00:00:00|0}
    [2018-07-05 14:57:53,443:][INFO ][T44] [缓存|期货保证金优惠明细表]变更通知成功：耗时=0,Type=FutureCombiPosPresenter,Method=FutureEnableDetail_AsyncChanged,opType=UPDATE,entity=513|800|824|9|pp1809|b|2|1|30354163|20180611|0|3|9396|0001-01-01 00:00:00|2018-07-05 14:57:53|0
    [2018-07-05 14:57:53,443:][INFO ][T44] [缓存|FutureEnableDetail]更新：writeRedo={True}, merged={True}, 原记录={513|800|824|9|pp1809|b|2|1|30354163|20180611|0|3|9396|0001-01-01 00:00:00|2018-07-05 14:57:53|0}, 待更新记录={513|800|824|9|pp1809|b|2|1|30354163|20180611|0|3|9396|0001-01-01 00:00:00|0001-01-01 00:00:00|0}
    [2018-07-05 14:57:53,446:][INFO ][T44] [缓存|FutureEnableDetail]收到消息并更新成功：主题={oplus.jy_tftrenabledetail}
    [2018-07-05 14:57:53,668:][INFO ][T42] [缓存|FutureRealdeal]收到更新消息(FutureRealdeal,oplus.jy_tfutrealdeal)，合计1行28列：
    第一行：fund_id=513,asset_id=800,combi_id=824,realdeal_serial_no=50001501,entrust_serial_no=2157,batch_serial_no=2156,report_code=pp1809,market_no=9,entrust_direction=35,deal_price=9110.000000000000,deal_amount=1.0000,deal_time=145805,total_fee=2.74,operator_no=1467,deal_balance=45550.00,ins_id=0,invest_type=b,trade_interface_type=6,deal_no=30563148,stockholder_id=03199477,close_type=1,cache_valid_flag=1,close_direction=0,cmborder_type=0,arbitrage_code=0,original_entrust_direction=35,entrust_launch_type=1,algo_ordid=

    [2018-07-05 14:57:53,668:][INFO ][T42] [缓存|期货成交表]变更通知成功：耗时=0,Type=OCacheGroupViewController`2,Method=cacheTable_Changed,opType=INSERT,entity=513|800|824|50001501|2157|2156|pp1809|9|35|9110|1|145805|2.74|1467|45550|0|b|6|30563148|03199477|1|0|0|0|35|0001-01-01 00:00:00|2018-07-05 14:57:53|0
    [2018-07-05 14:57:53,669:][INFO ][T42] [缓存|FutureRealdeal]收到消息并更新成功：主题={oplus.jy_tfutrealdeal}
    [2018-07-05 14:57:53,669:][INFO ][T42] [缓存|FutureEntrust]收到更新消息(FutureEntrust,oplus.jy_tfutentrusts)，合计1行43列：
    第十二行：arbitrage_serial_no=0,asset_id=800,batch_no=214,batch_serial_no=2156,branch_code=20018,cancel_amount=0.0000,cancel_serial_no=0,capital_account_no=30158889,close_direction=0,cmborder_type=0,combi_id=824,cum_avg_price=9110.000000000000,entrust_amount=60.0000,entrust_direction=35,entrust_price=9110.000000000000,entrust_price_type=0,entrust_serial_no=2157,entrust_status=6,entrust_time=145757,fund_id=513,ins_id=0,invest_type=b,last_deal_time=145805,market_no=9,min_volume=0.0000,operator_no=1467,pendoccupy_deposit_calctype=1,report_code=pp1809,report_direction=B,report_serial_no=111,report_serial_no_branch=31177631,revoke_cause= ,scheme_code= ,stockholder_id=03199477,total_deal_amount=5.0000,total_deal_balance=227750.00,trace_flag=2,trade_interface_type=6,confirm_codeno=119580149200678,entrust_launch_type=1,cache_valid_flag=1,algo_ordid=,auto_drop_flag= 

    [2018-07-05 14:57:53,669:][INFO ][T44] [缓存|FuturePosition]收到更新消息(FuturePosition,oplus.jy_tfutunitstock)，合计1行21列：
    第一行：market_no=9,report_code=pp1809,fund_id=513,asset_id=800,combi_id=824,invest_type=b,position_type=2,current_amount=816.0000,total_fee=338.32,today_close_profit=59495.00,total_close_profit=59495.00,open_cost=37802175.00,current_cost=37751310.00,close_enable_amount=761.0000,lastday_left_amount=814.0000,futures_used_margin=2642591.70,last_frozen_amount=814.0000,closed_profit_float=133625.00,original_real_cost=0.00,original_cost=43350305.00,cache_valid_flag=1";


        [TestMethod]
        public void TestMethod1()
        {
            string[] rlts = Regex.Split(text, @"\[\d{4}-\d{2}-\d{2} [0-2][0-9]:[0-5][0-9]:[0-5][0-9],[0-9]{3}:\]\[INFO \]");
        }

        [TestMethod]
        public void TestMatchs()
        {
            Regex re = new Regex(@"\[\d{4}-\d{2}-\d{2} [0-2][0-9]:[0-5][0-]:[0-5][0-9],[0-9]{3}:\]\[INFO \]\[T\d+\] \[缓存\|FutureEntrust\]收到更新消息\(FutureEntrust,oplus.jy_tfutentrusts\)，合计\d+行\d+列：\D*\[");
            List<string> datas = new List<string>();
            MatchCollection imgreg = re.Matches(text);
        }

        [TestMethod]
        public void TestGroupMatchs()
        {
            string input = @"[2018-07-06 09:31:04,230:]";
            string pattern = @"\[(?<FirstWord>\d{4}-\d{2}-\d{2}) [0-2][0-9]:[0-5][0-]:[0-5][0-9],[0-9]{3}:\]";
            Regex rgx = new Regex(pattern);
            Match match = rgx.Match(input);
            if (match.Success)
                ShowMatches(rgx, match);
        }

        private void ShowMatches(Regex r, Match m)
        {
            string[] names = r.GetGroupNames();
            Console.WriteLine("Named Groups:");
            foreach (var name in names)
            {
                Group grp = m.Groups[name];
                Console.WriteLine("   {0}: '{1}'", name, grp.Value);
            }
        }

        [TestMethod]
        public void TestReadFile()
        {
            string str = File.ReadAllText(@"e:\189300055-Cache-2018-07-05.log", Encoding.Default);
            List<string> timeStamps = new List<string>();
            List<string> dataLists = new List<string>();

            Regex re = new Regex(@"\[\d{4}-\d{2}-\d{2} [0-2][0-9]:[0-5][0-9]:[0-5][0-9],[0-9]{3}:\]");
            MatchCollection imgreg = re.Matches(str);

            string[] rlsts = Regex.Split(str, @"\[\d{4}-\d{2}-\d{2} [0-2][0-9]:[0-5][0-9]:[0-5][0-9],[0-9]{3}:\]\[INFO \]\[T\d+\] ");
            dataLists = new List<string>(rlsts);
            //dataLists.RemoveAt(0);

            List<CacheLogEntity> datas = new List<CacheLogEntity>();
            StringBuilder time = new StringBuilder();
            int timeLength = imgreg.Count;
            for (int i = 0; i < rlsts.Length; i++)
            {
                if (i >= timeLength)
                {
                    time.Clear();
                }
                else
                {
                    time.Clear().Append(imgreg[i].ToString());
                }
                datas.Add(new CacheLogEntity()
                {
                    TimeStamp = time.ToString(),
                    DataInfo = rlsts[i]
                });
            }
        }
    }

    public class CacheLogEntity
    {
        public string TimeStamp { get; set; }

        public string DataInfo { get; set; }
    }
}
