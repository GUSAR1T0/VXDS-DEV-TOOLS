module.exports = {
  root: true,
  env: {
    node: true
  },
  "extends": [
    "plugin:vue/essential",
    "eslint:recommended"
  ],
  rules: {
    quotes: [
      "error",
      "double"
    ],
    semi: [
      "error"
    ],
    camelcase: [
      "error",
      {
        "properties": "always"
      }
    ]
  },
  parserOptions: {
    parser: "babel-eslint"
  }
};
