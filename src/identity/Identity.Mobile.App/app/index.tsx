import { Link } from "expo-router";
import React from "react";
import { StyleSheet, Text, View } from "react-native";

export default function IndexTab() {
  return (
    <View style={styles.container}>
      <Text>Hello World</Text>
      <Link href="/scan" style={styles.btn}>
        Go to Scan screen
      </Link>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
  btn: {
    padding: 2,
    backgroundColor: "black",
  },
});
