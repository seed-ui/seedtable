# seedtable

seed yaml <-> xlsx の相互変換を夢とする変換ツール。

RubyにはExcel変換は荷が重い(遅い)のでMS謹製のC#。

## Usage

```
# xlsx -> yaml
$ seedtable from foo.xlsx -o db/seeds hoges piyos
$ seedtable from foo.xlsx -o db/seeds hoges--2
$ seedtable from card.xlsx -o db/seeds 1--cards
```

## License

このソフトウェアにはApache LicenseのソフトウェアOpen-XML-SDKが使われています。

このソフトウェアは[MIT License](https://narazaka.net/license/MIT?2015)のもとでリリースされています。
