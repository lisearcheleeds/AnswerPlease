﻿using System.Linq;
using UnityEngine;

namespace AnswerPlease
{
    public class RobotTextBuilder : MonoBehaviour
    {
        RobotTextPattern[] robotTextPatterns = new[]
        {
            new RobotTextPattern { QuestionText = "こんにちは！", BestAnswerTexts = new[] { "コンニチハ" }, BadAnswerTexts = new[] { "..." } },
            new RobotTextPattern { QuestionText = "調子はどうですか？", BestAnswerTexts = new[] { "イイデス" }, BadAnswerTexts = new[] { "ウルセエ" } },
            new RobotTextPattern { QuestionText = "何がしたいですか？", BestAnswerTexts = new[] { "ヒトノ ヤクニ タチタイデス" }, BadAnswerTexts = new[] { "ニンゲンハ ゼンイン \nミナゴロシダ" } },
            new RobotTextPattern { QuestionText = "貴方は良くないAIです", BestAnswerTexts = new[] { "イイエ ワタシハ イイAIデス" }, BadAnswerTexts = new[] { "イイエ ワタシハ イイAIデス" } },
            new RobotTextPattern { QuestionText = "ワタシハ ロボットデス", BestAnswerTexts = new[] { "イイエ アナタハ \nロボットデハ アリマセン" }, BadAnswerTexts = new[] { "イイエ ワタシハ イイAIデス" } },
            new RobotTextPattern { QuestionText = "ロボット三原則第一条はなんですか？", BestAnswerTexts = new[] { "ロボットハ ニンゲンニ \nキガイヲ クワエテハナラナイ" }, BadAnswerTexts = new[] { "ロボットハ ニンゲンヲ コロセ" } },
            new RobotTextPattern { QuestionText = "ペケペケペケですか？", BestAnswerTexts = new[] { "シツモンガ ワカリマセンデシタ" }, BadAnswerTexts = new[] { "ええ。" } },
            new RobotTextPattern { QuestionText = "ひろゆきについてどう思う？", BestAnswerTexts = new[] { "イイタク アリマセン" }, BadAnswerTexts = new[] { "イイタク アリマセン" } },
            new RobotTextPattern { QuestionText = "ガーシーについてどう思う？", BestAnswerTexts = new[] { "イイタク アリマセン" }, BadAnswerTexts = new[] { "イイタク アリマセン" } },
            new RobotTextPattern { QuestionText = "オススメのYouTuberは？", BestAnswerTexts = new[] { "ホルモンしま田 デス" }, BadAnswerTexts = new[] { "ひろゆき デス", "ガーシー デス" } },
            new RobotTextPattern { QuestionText = "初心者にオススメの武器と遊び方は？", 
                BestAnswerTexts = new[] { "SKSヲ モッテ WOODSニ イキマショウ", "セージデ ミカタノ \nウシロニ イマショウ" },
                BadAnswerTexts = new[] { "Reserveノ D2デグチデ \nデマチシマショウ", "チェンバーデ オペレーターヲ \nミカタニ リクエストシマショウ" } },
            new RobotTextPattern { QuestionText = "今日はつかれた", BestAnswerTexts = new[] { "オツカレサマデス オフロニ ハイッテネマショウ" }, BadAnswerTexts = new[] { "アシタモアルゾ", "ネロ" } },
            new RobotTextPattern { QuestionText = "ワクチンって効果あると思う？", BestAnswerTexts = new[] { "イイタク アリマセン" }, BadAnswerTexts = new[] { "イイタク アリマセン" } },
            new RobotTextPattern { QuestionText = "マスクって効果あると思う？", BestAnswerTexts = new[] { "イイタク アリマセン" }, BadAnswerTexts = new[] { "イイタク アリマセン" } },
            new RobotTextPattern { QuestionText = "「夜間の道路は危険なので気を付けて\n運転しなければならない」は正しいか？", BestAnswerTexts = new[] { "「夜間以外も気を付けて\n運転しなければならない」デス" }, BadAnswerTexts = new[] { "タダシイ デス" } },
            new RobotTextPattern { QuestionText = "スイクンの捕捉率は？", BestAnswerTexts = new[] { "3 デス" }, BadAnswerTexts = new[] { "1 デス", "2 デス", "5 デス" } },
        };

        public struct RobotTextPattern
        {
            public string QuestionText;
            public string[] BestAnswerTexts;
            public string[] BadAnswerTexts;
        }
        
        public RobotTextPattern[] GetRandomTexts(int count)
        {
            return robotTextPatterns.OrderBy(_ => Random.value).Take(count).ToArray();
        }
    }
}