# seedtable

seed yaml <-> xlsx の相互変換を夢とする変換ツール。

RubyにはExcel変換は荷が重い(遅い)のでMS謹製のC#。

## Usage

```
# xlsx -> yaml
$ seedtable from quest.xlsx -o db/seeds stages quests
$ seedtable from limit_break.xlsx -o db/seeds limit_break_materials--2
$ seedtable from card.xlsx -o db/seeds 1--cards
```

## License

このソフトウェアにはApache LicenseのソフトウェアOpen-XML-SDKが使われています。

このソフトウェアは[MIT License](https://narazaka.net/license/MIT?2015)のもとでリリースされています。
