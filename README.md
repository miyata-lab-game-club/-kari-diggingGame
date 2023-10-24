# 仮モック的なもの
# diggingGame
穴掘りRPGをつくる
# Notion企画書：
https://www.notion.so/RPG-b69dfb6e6a974543af4b05076eceaf71
# システム
## 穴掘りパート
### 全体の概要
街を見つける穴掘りパートはプレイヤーが自由に穴を掘っていく。たまにお宝が見つかるかも。プレイヤーが気の向くままに掘っていき、街を見つけていく。周辺をスキャンすると周辺に埋まっている宝や町の情報を得ることができる。プレイヤーには重力が存在するので始めは斜めに掘っていかないと上に行けない。ストーリーが進むと真上に進むことができる機能も付く予定。プレイヤーは休憩スポットを作りそこにベッドを置くことで進行状況をセーブすることができる。街でもセーブすることが可能。ストーリー上必ず通らなければいけない街には妖精が案内してくれる。

![穴掘りゲーム_アートボード 1](https://github.com/miyata-lab-game-club/diggingGame/assets/66520685/6be191e6-6f5a-4650-b78b-46ae282716dd)
### 戦闘
穴掘りをしている最中には戦闘がある。戦闘システムは穴掘り×コマンドバトルで、以下の手順を踏む。 

１．ターンの初めに穴掘りパネルを操作して爆弾や毒薬などわなを仕掛ける。アイテムは消費するもので穴掘り中に道中で拾う。 
![穴掘りゲーム  復元 -02](https://github.com/miyata-lab-game-club/diggingGame/assets/66520685/076119b4-69bd-458c-8d54-d3e067a96aab)

２．コマンドバトルの開始 
ここでプレイヤー側の技で敵の位置を変えることができ、罠まで追い詰める。 

１ターン目：

![穴掘りゲーム２-02](https://github.com/miyata-lab-game-club/diggingGame/assets/66520685/7e7bbf42-7745-45ab-b3d7-c5ce7029c270)

２ターン目：

![穴掘りゲーム３-02](https://github.com/miyata-lab-game-club/diggingGame/assets/66520685/75b7336a-761c-439b-be01-9bb1e16ad5d9)

図の場合だと敵の位置に爆弾が仕掛けられているので敵はダメージを受ける。ただ穴を掘った場合、敵を穴にはめることができ、敵のターンが１ターン消失する。どのタイミングでどこに敵をいどうさせるかという戦略性のあるゲーム。 
## 街を探索するパート
街を探索しながらそこで暮らす住人の話を聞く。そして住人は、あるときはおつかい、あるときは温泉堀、あるときは討伐などさまざまなお願いをしてくる。それらの願い事を聞いてその街の住人達からの信頼を得る。そうすることで地上を目指す主人公に強く共感してくれるようになり、彼らも地上に出ることを決意してくれる。また、住人がなぜ地底に暮らしているのかを聞く。メインストーリーの必ず通らなければいけない街だけを通った場合とメインストーリー外の自分で見つけた街の住人の話を聞くと話に聞いた歴史とは違う側面を見ることができる。
