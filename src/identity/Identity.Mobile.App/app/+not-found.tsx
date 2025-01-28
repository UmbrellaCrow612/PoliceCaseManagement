import { StyleSheet, Text, View } from "react-native";

export default function NotFoundTab() {
  return (
    <View style={styles.container}>
      <Text style={styles.text}>404 not found</Text>
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
  text: {
    fontSize: 30,
    fontWeight: "bold",
  },
});
