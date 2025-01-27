import React from "react";
import { StyleSheet, Text, View } from "react-native";



/*

How it works:

User clicks option on idex nav menu bar
An option would be scan OTP Code 
Then it gets data and pushes to thus page with the details 
This would be a stateless and dummy page basically 


*/


export default function OTPTab() {
  return (
    <View style={styles.container}>
      <Text>Hello World</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  }
});
