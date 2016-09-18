# seedtable

seed yaml <-> xlsx を相互変換するツール

## Usage

```
# xlsx -> yaml
$ seedtable from foo.xlsx -o db/seeds
$ seedtable from foo.xlsx -o db/seeds -O hoges piyos
$ seedtable from foo.xlsx -o db/seeds -S hoges:2
$ seedtable from foo.xlsx -o db/seeds -S 1:hoges
$ seedtable from -i doc foo.xlsx -o db/seeds

# yaml -> xlsx
$ seedtable to -s db/seeds -x doc foo.xlsx -o newdoc
```

## Engines

seedtableではxlsxファイルを扱うバックエンドとして複数のライブラリを選択できます。

`-e`オプションに指定してください。

| ライブラリ | from (xlsx -> yaml) | to (yaml -> xlsx) |
|---|---|---|
| OpenXml | 速い | 行挿入削除× |
| EPPlus | 速い | 行挿入削除〇 |
| ClosedXML | 遅い 一部数式を含むxlsxはエラー | 行挿入削除× スタイルが保存されない |

OpenXmlとEPPlusのfrom処理速度はほぼ同じですが、ファイルによって3割程度の速度差がある場合があります。
ClosedXMLはその2倍以上程度の時間がかかります。

## 未実装機能

- `-R, --require-version`オプションは動作しません
- to処理においてsubdivideは考慮されません
- `-d, --delete`オプションはEPPlusエンジンでのみ動作します

## Excelファイルの形式

以下のような形式を想定しています。

### Excel

シート名`characters`

|   | A  |  B   |      C      |
|---|----|------|-------------|
| 1 | ID | 名前 |    説明     |
| 2 | id | name | description |
| 3 | 1 | さくら | F.I.R.S.T |
| 4 | 2 | アルル | ボク |
| 5 | 3 | さっちゃんさん | 髪が長い |

### YAML

`characters.yml`

```
data1:
  id: 1
  name: さくら
  description: F.I.R.S.T
data2:
  id: 2
  name: アルル
  description: ボク
data3:
  id: 3
  name: さっちゃんさん
  description: 髪が長い
```

デフォルトではExcel表の2行目がカラム名、3行目以降がデータとして扱われますが、これらはそれぞれ`--column-names-row`、`--data-start-row`オプションで変更可能です。

## License

このソフトウェアにはApache Licenseのもとで頒布されているソフトウェアOpen-XML-SDKと、LGPLのもとで頒布されているソフトウェアEPPlusが使われています。

このソフトウェアは[MIT License](https://narazaka.net/license/MIT?2016)のもとでリリースされています。
